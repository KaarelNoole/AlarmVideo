using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;
using System.Windows.Controls;
using Location = Microsoft.Maps.MapControl.WPF.Location;
using VideoOS.Platform.Proxy.AlarmClient;
using Microsoft.Maps.MapControl.WPF;
using System.Collections.Generic;
using VideoOS.Platform.UI;
using System.Windows.Media.Imaging;
using System.Linq;
using MessageBox = System.Windows.MessageBox;
using System.Data.SqlClient;
using System.Windows.Threading;
using System.Windows.Media;
using System.Globalization;
using VideoOS.Platform.EventsAndState;
using Microsoft.Extensions.Logging;
using VideoOS.Platform.Data;
using GMap.NET.MapProviders;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow
    {
        private Item _selectItem1;
        private AlarmClientManager _alarmClientManager;
        private Alarm _selectedAlarm = null;
        private List<Alarm> _alarms;
        private DispatcherTimer timer;
        private List<Alarm> closedAlarms = new List<Alarm>();
        private List<Alarm> activeAlarms = new List<Alarm>();
        private List<EventItem> eventItemList = new List<EventItem>();
        private bool isTimerTickEvent = false;

        public MainWindow()
        {
            InitializeComponent();
            _alarmClientManager = new AlarmClientManager();

            double initialLatitude = 58.883333;
            double initialLongitude = 25.557222;
            double initialZoomLevel = 6;
            Location initialLocation = new Location(initialLatitude, initialLongitude);

            mapControl.Center = initialLocation;
            mapControl.ZoomLevel = initialZoomLevel;

            DataContext = this;


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30); 
            timer.Tick += Timer_Tick;
            timer.Start();

            LoadClientAlarmsToListBox();
            _alarms = new List<Alarm>();
            InitializeListBox();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //if (!isTimerTickEvent)
            //{
            //    LoadClientAlarmsToListBox();
            //}
            LoadClientAlarmsToListBox();
        }

        //private void ActiveAlarms(object sender, RoutedEventArgs e)
        //{
        //    HandleActiveAlarmsClick();
        //}


        //private void HandleActiveAlarmsClick()
        //{
        //    isTimerTickEvent = true;
        //    LoadClientAlarmsToListBox();
        //    isTimerTickEvent = false;
        //}

        //AlarmsListBox alarmite lisamine 
        private void LoadClientAlarmsToListBox()
        {
            try
            {
                string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT EventTime, Source, Event, Id FROM Alarm WHERE Status IS NULL /*AND Status NOT Like '%Accepted%'*/";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            alarmsListBox.Items.Clear();

                            while (reader.Read())
                            {

                                if (!reader.IsDBNull(0))
                                {

                                    DateTime eventTime = reader.GetDateTime(0);

                                    string source = reader.GetString(1);
                                    string eventType = reader.GetString(2);
                                    int Id = reader.GetInt32(3);

                                    alarmsListBox.Items.Add(new Alarm
                                    {

                                        EventTime = eventTime,
                                        Source = source,
                                        Event = eventType,
                                        Id = Id,
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("LoadClientAlarmsToListBox", ex);
            }
        }

        private void InitializeListBox()
        {
            alarmsListBox.SelectionChanged += AlarmsListBox_SelectionChanged;
        }

        //alarmide valimine
        private void AlarmsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (alarmsListBox.SelectedItem != null)
            {
                _selectedAlarm = alarmsListBox.SelectedItem as Alarm;

                _selectedAlarm.Id = ((Alarm)alarmsListBox.SelectedItem).Id;

                eventItemList.Clear();

                try
                {
                    string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT Comment, CommentTime FROM Comments WHERE AlarmId = @AlarmId";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@AlarmId", _selectedAlarm.Id);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                                    {
                                        string comment = reader.GetString(0);
                                        DateTime commentTime = reader.GetDateTime(1);
                                        eventItemList.Add(new EventItem { Comment = comment, CommentTime = commentTime });
                                    }
                                }
                            }
                        }
                    }

                    EventListBox.ItemsSource = null;
                    EventListBox.ItemsSource = eventItemList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Viga kommentaaride laadimisel: {ex.Message}");
                }
            }
        }



        //private void HandleSelectionChange()
        //{
        //    eventItemList.Clear();

        //    if (_selectedAlarm != null)
        //    {
        //        ListBoxItem selectedItem = (ListBoxItem)alarmsListBox.ItemContainerGenerator.ContainerFromItem(_selectedAlarm);
        //        if (selectedItem != null)
        //        {
        //            TextBlock eventTimeTextBlock = GetChildOfType<TextBlock>(selectedItem, "eventTimeTextBlock");
        //            TextBlock sourceTextBlock = GetChildOfType<TextBlock>(selectedItem, "sourceTextBlock");
        //            TextBlock eventTextBlock = GetChildOfType<TextBlock>(selectedItem, "eventTextBlock");

        //            if (eventTimeTextBlock != null)
        //            {
        //                eventTimeTextBlock.FontSize = 16;
        //            }
        //        }
        //    }

        //    EventListBox.ItemsSource = null;
        //    EventListBox.ItemsSource = eventItemList;
        //}

        //private T GetChildOfType<T>(DependencyObject depObject, string name) where T : DependencyObject
        //{
        //    if (depObject == null) return null;
        //    int count = VisualTreeHelper.GetChildrenCount(depObject);
        //    for (int i = 0; i < count; i++)
        //    {
        //        var child = VisualTreeHelper.GetChild(depObject, i);
        //        if (child != null && child is T && ((FrameworkElement)child).Name == name)
        //        {
        //            return (T)child;
        //        }
        //        var result = GetChildOfType<T>(child, name);
        //        if (result != null) return result;
        //    }
        //    return null;
        //}

        //Kommentaaride lisamise nupp 
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (alarmsListBox.SelectedItem != null)
            {
                string enteredText = alarmDetailsTextBox.Text;
                if (!string.IsNullOrWhiteSpace(enteredText))
                {
                    
                    if (alarmsListBox.SelectedItem is Alarm selectedAlarm)
                    {
                        
                        int selectedAlarmId = selectedAlarm.Id;

                        
                        SaveCommentToDatabase(enteredText, selectedAlarmId);

                        EventItem newItem = new EventItem { Comment = enteredText };

                        eventItemList.Add(newItem);

                        EventListBox.Items.Refresh();

                        alarmDetailsTextBox.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Palun vali alarm, et lisada kommentaar.");
            }
        }

        //Kommentaaride salvestamine andmebaasi
        private void SaveCommentToDatabase(string comment, int alarmId)
        {
            if (_selectedAlarm != null)
            {
                try
                {
                    string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO Comments (AlarmId, Comment, CommentTime) VALUES (@Id, @Comment, GETDATE())";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", alarmId);
                            command.Parameters.AddWithValue("@Comment", comment);

                            command.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Alarmile lisamisel kommentaar ilmnes viga: {ex.Message}");
                }
            }
        }

        //Aktseteerimise nupp
        private void acceptAlarmsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAlarm != null)
            {
                try
                {
                    string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string updateQuery = "UPDATE Alarm SET Status = @Status WHERE EventTime = @EventTime AND Source = @Source AND Event = @Event";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@Status", "Accepted");
                            updateCommand.Parameters.AddWithValue("@EventTime", _selectedAlarm.EventTime);
                            updateCommand.Parameters.AddWithValue("@Source", _selectedAlarm.Source);
                            updateCommand.Parameters.AddWithValue("@Event", _selectedAlarm.Event);

                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                alarmsListBox.Items.Remove(_selectedAlarm);
                                activeAlarms.Add(_selectedAlarm);
                                eventItemList.Add(new EventItem { Comment = "Alarm aktsepteeritud" });
                                EventListBox.ItemsSource = null;
                                EventListBox.ItemsSource = eventItemList;
                                MessageBox.Show("Alarmi oleku värskendamine õnnestus.");

                                string insertCommentQuery = "INSERT INTO Comments (AlarmId, Comment, CommentTime) VALUES (@AlarmId, @Comment, GETDATE())";
                                using (SqlCommand insertCommentCommand = new SqlCommand(insertCommentQuery, connection))
                                {
                                    insertCommentCommand.Parameters.AddWithValue("@AlarmId", _selectedAlarm.Id);
                                    insertCommentCommand.Parameters.AddWithValue("@Comment", "Alarm aktsepteeritud");
                                    insertCommentCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Alarmi oleku värskendamine ebaõnnestus. Alarmi ei leitud.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Alarmi värskendamisel ilmnes viga: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Valige Alarm, et selle olekut värskendada.");
            }
        }

        private void EventAlarmsButton_Click(object sender, RoutedEventArgs e)
        {
            var events = new List<Event>();

            foreach (var item in eventItemList)
            {
                var newEvent = new Event
                {
                    Time = DateTime.Now,
                    Comment = item.Comment, 
                    CommentTime = DateTime.Now,
                    AlarmEnd = DateTime.Now.AddMinutes(10),
                    EventEnd = DateTime.Now.AddMinutes(20),
                    Event_Recovery_time = DateTime.Now.AddMinutes(25)
                };

                events.Add(newEvent);
            }

            ProcessEvents(events);
        }

        private void ProcessEvents(IEnumerable<Event> events)
        {
            string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var ev in events)
                {
                    string insertQuery = "INSERT INTO Event (Time, Comment, CommentTime, AlarmEnd, EventEnd, Event_Recovery_time) " +
                                         "VALUES (@Time, @Comment, @CommentTime, @AlarmEnd, @EventEnd, @Event_Recovery_time)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);

                    
                    command.Parameters.AddWithValue("@Time", ev.Time);
                    command.Parameters.AddWithValue("@Comment", ev.Comment);
                    command.Parameters.AddWithValue("@CommentTime", ev.CommentTime);
                    command.Parameters.AddWithValue("@AlarmEnd", ev.AlarmEnd);
                    command.Parameters.AddWithValue("@EventEnd", ev.EventEnd);
                    command.Parameters.AddWithValue("@Event_Recovery_time", ev.Event_Recovery_time);

                    
                    command.ExecuteNonQuery();
                }
            }
        }
        //Alarmide kustutamine
        private void WrongAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAlarm != null)
            {
                try
                {
                    string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string formattedEventTime = _selectedAlarm.EventTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                        string query = $"DELETE FROM Alarm WHERE EventTime = '{formattedEventTime}' AND Source = '{_selectedAlarm.Source}' AND Event = '{_selectedAlarm.Event}'";
                        
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                LoadClientAlarmsToListBox();
                                MessageBox.Show("Alarm kustutati andmebaasist edukalt.");
                            }
                            else
                            {
                                MessageBox.Show("Alarmi kustutamine andmebaasist ebaõnnestus. Alarmi ei leitud");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Alarmi kustutamisel ilmnes viga: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Palun vali alarm ,et kustutada");
            }
        }

        private void AlarmRequestButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SendVideoButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        //Alarmi lõpetus nupp
        private void AlarmClosedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAlarm != null)
            {
                try
                {
                    string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string updateQuery = "UPDATE Alarm SET Status = @Status WHERE EventTime = @EventTime AND Source = @Source AND Event = @Event";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@Status", "Closed");
                            updateCommand.Parameters.AddWithValue("@EventTime", _selectedAlarm.EventTime);
                            updateCommand.Parameters.AddWithValue("@Source", _selectedAlarm.Source);
                            updateCommand.Parameters.AddWithValue("@Event", _selectedAlarm.Event);

                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                alarmsListBox.Items.Remove(_selectedAlarm);
                                closedAlarms.Add(_selectedAlarm);
                                eventItemList.Add(new EventItem { Comment = "Alarm lõpetatud" });
                                EventListBox.ItemsSource = null;
                                EventListBox.ItemsSource = eventItemList;
                                MessageBox.Show("Alarmi oleku värskendamine õnnestus.");

                                string insertCommentQuery = "INSERT INTO Comments (AlarmId, Comment, CommentTime) VALUES (@AlarmId, @Comment, GETDATE())";
                                using (SqlCommand insertCommentCommand = new SqlCommand(insertCommentQuery, connection))
                                {
                                    insertCommentCommand.Parameters.AddWithValue("@AlarmId", _selectedAlarm.Id);
                                    insertCommentCommand.Parameters.AddWithValue("@Comment", "Alarm lõpetatud");
                                    insertCommentCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Alarmi oleku värskendamine ebaõnnestus. Alarmi ei leitud.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Alarmi värskendamisel ilmnes viga: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Valige Alarm, et selle olekut värskendada.");
            }
        }
        //Aktsepteeritud alarmi List nupp
        private void ActiveAlarms_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT DISTINCT a.Id, a.EventTime, a.Source, a.Event " +
                                   "FROM Alarm a " +
                                   "LEFT JOIN Comments c ON a.Id = c.AlarmId " +
                                   "WHERE a.Status = 'Accepted'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            alarmsListBox.Items.Clear();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    int id = reader.GetInt32(0); 
                                    DateTime eventTime = reader.GetDateTime(1);
                                    string source = reader.GetString(2);
                                    string eventType = reader.GetString(3);

                                    
                                    alarmsListBox.Items.Add(new Alarm
                                    {
                                        Id = id,
                                        EventTime = eventTime,
                                        Source = source,
                                        Event = eventType
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("ActiveAlarms_Click", ex);
            }
        }

        private void WorkAlarms_Click(object sender, RoutedEventArgs e)
        {
            alarmsListBox.Items.Clear();
        }

        //Lõpetatud alarmi List nupp
        private void ClosedAlarms_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT a.Id, a.EventTime, a.Source, a.Event " +
                                   "FROM Alarm a " +
                                   "LEFT JOIN Comments c ON a.Id = c.AlarmId " +
                                   "WHERE a.Status = 'Closed'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            alarmsListBox.Items.Clear();
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    int id = reader.GetInt32(0);
                                    DateTime eventTime = reader.GetDateTime(1);
                                    string source = reader.GetString(2);
                                    string eventType = reader.GetString(3);


                                    alarmsListBox.Items.Add(new Alarm
                                    {
                                        Id = id,
                                        EventTime = eventTime,
                                        Source = source,
                                        Event = eventType
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("ActiveAlarms_Click", ex);
            }
        }

        private void MyAlarms_Click(object sender, RoutedEventArgs e)
        {
            alarmsListBox.Items.Clear();
        }
        //Kõik alarme millega pole veel alastatud
        private void AllAlarms_Click(object sender, RoutedEventArgs e)
        {
            LoadClientAlarmsToListBox();
        }

        private void Button_Select1_Click(object sender, RoutedEventArgs e)
        {
            _imageViewerWpfControl.Disconnect();
            _imageViewerWpfControl.Close();

            ItemPickerWpfWindow itemPicker = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid> { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                Items = Configuration.Instance.GetItems()
            };

            if (itemPicker.ShowDialog().Value)
            {
                _selectItem1 = itemPicker.SelectedItems.First();

                if (_selectItem1 != null)
                {
                    SetupMapAndMarker();
                    SetupImageViewer();
                }
            }
        }

        private double GetLatitudeFromSDK(Item cameraItem)
        {
            if (cameraItem != null && cameraItem.Properties != null)
            {
                if (cameraItem.Properties.TryGetValue("Latitude", out var latitudeValue) &&
                    double.TryParse(latitudeValue.ToString(), out var latitude))
                {
                    return latitude;
                }
            }
            return 0.0;
        }

        private double GetLongitudeFromSDK(Item cameraItem)
        {
            if (cameraItem != null && cameraItem.Properties != null)
            {
                if (cameraItem.Properties.TryGetValue("Longitude", out var longitudeValue) &&
                    double.TryParse(longitudeValue.ToString(), out var longitude))
                {
                    return longitude;
                }
            }
            return 0.0;
        }

        private void SetupMapAndMarker()
        {
            double initialLatitude = GetLatitudeFromSDK(_selectItem1);
            double initialLongitude = GetLongitudeFromSDK(_selectItem1);
            double initialZoomLevel = 16;

            Location initialLocation = new Location(initialLatitude, initialLongitude);

            mapControl.Center = initialLocation;
            mapControl.ZoomLevel = initialZoomLevel;
            var customIcon = new BitmapImage(new Uri("/icon/icon.png", UriKind.Relative));

            var marker = new Image
            {
                Source = customIcon,
                Width = 30,
                Height = 30
            };

            MapLayer.SetPosition(marker, initialLocation);

            mapControl.Children.Add(marker);

            buttonSelect1.Content = _selectItem1.Name;
        }

        private void SetupImageViewer()
        {

            _imageViewerWpfControl.CameraFQID = _selectItem1.FQID;
            _imageViewerWpfControl.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
            _imageViewerWpfControl.EnableVisibleLiveIndicator = EnvironmentManager.Instance.Mode == Mode.ClientLive;
            _imageViewerWpfControl.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;
            _imageViewerWpfControl.Initialize();
            _imageViewerWpfControl.Connect();
            _imageViewerWpfControl.Selected = true;
            _imageViewerWpfControl.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
        }

        private void ImageViewerWpfControl1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _imageViewerWpfControl.Selected = true;
        }

        void ImageOrPaintChangedHandler(object sender, EventArgs e)
        {
            Debug.WriteLine("ImageSize:" + _imageViewerWpfControl.ImageSize.Width + "x" + _imageViewerWpfControl.ImageSize.Height + ", PaintSIze:" +
            _imageViewerWpfControl.PaintSize.Width + "x" + _imageViewerWpfControl.PaintSize.Height +
            ", PaintLocation:" + _imageViewerWpfControl.PaintLocation.X + "-" + _imageViewerWpfControl.PaintLocation.Y);
        }

        private void ButtonStartRecording1_Click(object sender, RoutedEventArgs e)
        {
            if (_selectItem1 != null)
                EnvironmentManager.Instance.PostMessage(
                    new Message(MessageId.Control.StartRecordingCommand), _selectItem1.FQID);
        }

        private void ButtonStopRecording1_Click(object sender, RoutedEventArgs e)
        {
            if (_selectItem1 != null)
                EnvironmentManager.Instance.PostMessage(
                    new Message(MessageId.Control.StopRecordingCommand), _selectItem1.FQID);
        }

        private void checkBoxHeader_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCheckBoxHeader();
        }

        private void CheckBoxHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateCheckBoxHeader();
        }

        private void checkBoxDigitalZoom_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCheckBoxDigitalZoom();
        }

        private void CheckBoxDigitalZoom_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateCheckBoxDigitalZoom();
        }

        private void checkBoxAdaptiveStreaming_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCheckBoxAdaptiveStreaming();
        }

        private void CheckBoxAdaptiveStreaming_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateCheckBoxAdaptiveStreaming();
        }

        private void UpdateCheckBoxHeader()
        {
            _imageViewerWpfControl.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
        }

        private void UpdateCheckBoxDigitalZoom()
        {
            _imageViewerWpfControl.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
        }

        private void UpdateCheckBoxAdaptiveStreaming()
        {
            _imageViewerWpfControl.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;
        }
        private void alarmDetailsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = alarmDetailsTextBox.Text;

            EventItem newItem = new EventItem { Comment = enteredText };

            EventListBox.ItemsSource = null;

            EventListBox.Items.Add(newItem);
        }
        private void EventAlarmsUpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}