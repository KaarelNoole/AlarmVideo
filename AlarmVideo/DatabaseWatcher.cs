using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VideoOS.Platform.Data;

namespace AlarmVideo
{
    public class DatabaseWatcher
    {
        private string _connectionString;
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDependency _dependency;
        private ListBox _alarmsListBox;

        public event EventHandler<AlarmEventArgs> NewAlarmAdded;

        public DatabaseWatcher(string connectionString, ListBox listBox)
        {
            _connectionString = connectionString;
            _alarmsListBox = listBox;

            _connection = new SqlConnection(_connectionString);
            _command = new SqlCommand("SELECT Id, EventTime, Source, Event FROM Alarm WHERE Status IS NULL", _connection);
            _dependency = new SqlDependency(_command);
            _dependency.OnChange += Dependency_OnChange;
        }

        public void StartWatching()
        {
            SqlDependency.Start(_connectionString);

            _connection.Open();

            _command.Notification = null;

            SqlDependency dependency = new SqlDependency(_command);
            dependency.OnChange += Dependency_OnChange;

            _command.ExecuteReader();
        }

        public void StopWatching()
        {
            _connection.Close();
            _command.Dispose();
            _dependency.OnChange -= Dependency_OnChange;
        }

        private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                Console.WriteLine("Andmebaasis toimusid muudatused!");

                if (e.Info == SqlNotificationInfo.Insert)
                {
                    LoadDataToListBox();
                }
            }
        }

        private void LoadDataToListBox()
        {
            try
            {
                List<Alarm> alarms = new List<Alarm>();

                string query = "SELECT EventTime, Source, Event, Id FROM Alarm WHERE Status IS NULL";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alarm alarm = new Alarm
                            {
                                EventTime = reader.GetDateTime(0),
                                Source = reader.GetString(1),
                                Event = reader.GetString(2),
                                Id = reader.GetInt32(3)
                            };

                            alarms.Add(alarm);
                        }
                    }
                }

                NewAlarmAdded?.Invoke(this, new AlarmEventArgs(alarms));

                _alarmsListBox.Dispatcher.Invoke(() =>
                {
                    _alarmsListBox.ItemsSource = alarms;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Viga andmete laadimisel: {ex.Message}");
            }
        }
    }
}

