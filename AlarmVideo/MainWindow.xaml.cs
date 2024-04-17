using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.AlarmClient;
using VideoOS.Platform.UI;
using VideoOS.Platform.UI.Controls;
using Location = Microsoft.Maps.MapControl.WPF.Location;
using MessageBox = System.Windows.MessageBox;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow, INotifyPropertyChanged
    {
        private Item _selectItem;
        private AlarmClientManager _alarmClientManager;
        private Alarm _selectedAlarm;
        private List<Alarm> closedAlarms = new List<Alarm>();
        private List<Alarm> activeAlarms = new List<Alarm>();
        private List<EventItem> eventItemList = new List<EventItem>();
        private DispatcherTimer _timer;
        public event EventHandler NewAlarmAdded;
        private DatabaseWatcher _databaseWatcher;
        private bool _allowListBoxUpdate = true;
        private double _speed = 0.0;




        private ObservableCollection<Alarm> _alarms;
        public ObservableCollection<Alarm> Alarms
        {
            get { return _alarms; }
            set
            {
                _alarms = value;
                OnPropertyChanged(nameof(Alarms));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeListBox();
            _alarmClientManager = new AlarmClientManager();

            double initialLatitude = 58.883333;
            double initialLongitude = 25.557222;
            double initialZoomLevel = 6;
            Location initialLocation = new Location(initialLatitude, initialLongitude);

            mapControl.Center = initialLocation;
            mapControl.ZoomLevel = initialZoomLevel;

            DataContext = this;
            _alarms = new ObservableCollection<Alarm>();
            LoadClientAlarmsToListBox();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
            _databaseWatcher = new DatabaseWatcher(connectionString, alarmsListBox);
            _databaseWatcher.NewAlarmAdded += NewAlarmAddedHandler;
            _databaseWatcher.StartWatching();

            EnvironmentManager.Instance.RegisterReceiver(PlaybackTimeChangedHandler, new MessageIdFilter(MessageId.SmartClient.PlaybackCurrentTimeIndication));
        }

        private void OnNewAlarmAdded()
        {
            NewAlarmAdded?.Invoke(this, EventArgs.Empty);
        }

        private void NewAlarmAddedHandler(object sender, EventArgs e)
        {
            LoadClientAlarmsToListBox();
        }

        private void LoadNewDataFromDatabase()
        {
            
            if (!_allowListBoxUpdate) return; 

            try
            {
                ObservableCollection<Alarm> loadedAlarms = new ObservableCollection<Alarm>();

                string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT EventTime, Source, Event, Id FROM Alarm WHERE Status IS NULL";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Alarm newAlarm = new Alarm
                                {
                                    Id = reader.GetInt32(3),
                                    EventTime = reader.GetDateTime(0),
                                    Source = reader.GetString(1),
                                    Event = reader.GetString(2)
                                };

                                if (!alarmsListBox.Items.Cast<Alarm>().Any(alarm => alarm.Id == newAlarm.Id))
                                {
                                    loadedAlarms.Add(newAlarm);
                                    alarmsListBox.Items.Add(newAlarm);
                                }
                            }
                        }
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    Alarms.Clear();
                    foreach (var alarm in loadedAlarms)
                    {
                        Alarms.Add(alarm);
                    }
                    OnNewAlarmAdded();
                });
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("LoadNewDataFromDatabase", ex);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadNewDataFromDatabase();
        }

        //AlarmsListBox alarmite lisamine 
        public void LoadClientAlarmsToListBox()
        {
            _allowListBoxUpdate = true;

            Dispatcher.Invoke(() =>
            {
                if (alarmsListBox.ItemsSource != null)
                {
                    alarmsListBox.ItemsSource = null;
                }

                alarmsListBox.Items.Clear();

                try
                {
                    string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT EventTime, Source, Event, Id FROM Alarm WHERE Status IS NULL";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
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
            });
        }


        private void InitializeListBox()
        {
            alarmsListBox.SelectionChanged += AlarmsListBox_SelectionChanged;
        }

        //alarmide valimine
        private void AlarmsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (alarmsListBox.SelectedItem is Alarm selectedAlarm)
            {
                //_selectItem = alarmsListBox.SelectedItem as Item;
                _selectedAlarm = alarmsListBox.SelectedItem as Alarm;

                _selectedAlarm.Id = ((Alarm)alarmsListBox.SelectedItem).Id;
                _selectedAlarm.Source = ((Alarm)alarmsListBox.SelectedItem).Source;
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
                    SetupImageViewer();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Viga kommentaaride laadimisel: {ex.Message}");
                }
            }
        }
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
        private  void AlarmClosedButton_Click(object sender, RoutedEventArgs e)
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
                                // await DelayToDatabaseAsync();
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
        private  void ActiveAlarms_Click(object sender, RoutedEventArgs e)
        {
            _allowListBoxUpdate = false;

            if (alarmsListBox.ItemsSource != null)
            {
                alarmsListBox.ItemsSource = null;
            }

            alarmsListBox.Items.Clear();

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
                            var loadedAlarms = new List<Alarm>();

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

                            if (_allowListBoxUpdate)
                            {
                                LoadClientAlarmsToListBox();
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
            _allowListBoxUpdate = false;
            alarmsListBox.ItemsSource = null;
            alarmsListBox.Items.Clear();

            if (_allowListBoxUpdate)
            {
                LoadClientAlarmsToListBox();
            }
        }

        //Lõpetatud alarmi List nupp
        private void ClosedAlarms_Click(object sender, RoutedEventArgs e)
        {
            _allowListBoxUpdate = false;

            if (alarmsListBox.ItemsSource != null)
            {
                alarmsListBox.ItemsSource = null;
            }
            alarmsListBox.Items.Clear();

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

                            if (_allowListBoxUpdate)
                            {
                                LoadClientAlarmsToListBox();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("ClosedAlarms_Click", ex);
            }
        }
        private void MyAlarms_Click(object sender, RoutedEventArgs e)
        {
            _allowListBoxUpdate = false;
            alarmsListBox.ItemsSource = null;
            alarmsListBox.Items.Clear();

            if (_allowListBoxUpdate)
            {
                LoadClientAlarmsToListBox();
            }
        }
        //Kõik alarme millega pole veel alastatud
        private void AllAlarms_Click(object sender, RoutedEventArgs e)
        {
            LoadClientAlarmsToListBox();
        }
        //Kaamera valimise nupp
        private void Button_Select_Click(object sender, RoutedEventArgs e)
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
                _selectItem = itemPicker.SelectedItems.First();

                if (_selectItem != null)
                {
                    SetupMapAndMarker(_selectItem);
                    SetupImageViewer();
                }
            }
        }

        private void GetLongitudeAndLatitude(Item item, VideoOSTreeViewItem tn)
        {
            VideoOSTreeViewItem fields = new VideoOSTreeViewItem()
            {
                Data = "Fields",
                IsExpanded = true,
            };

            var fieldsChildren = new List<VideoOSTreeViewItem>()
            {
                new VideoOSTreeViewItem() { Data = "HasRelated : " + item.HasRelated },
                new VideoOSTreeViewItem() { Data = "HasChildren: " + item.HasChildren },
            };

            if (item.PositioningInformation == null)
            {
                fieldsChildren.Add(new VideoOSTreeViewItem() { Data = "No PositioningInformation" });
            }
            else
            {
                fieldsChildren.Add(new VideoOSTreeViewItem()
                {
                    Data = "PositioningInformation: Latitude=" + item.PositioningInformation.Latitude + ", Longitude=" + item.PositioningInformation.Longitude
                });
            }

            fields.Children = fieldsChildren;
            if (tn.Children != null)
            {
                tn.Children.Add(fields);
            }
            else
            {
                tn.Children = new List<VideoOSTreeViewItem> { fields };
            }
        }

        //Kaart
        private void SetupMapAndMarker(Item item)
        {
            if (item != null && item.PositioningInformation != null)
            {
                double latitude = item.PositioningInformation.Latitude;
                double longitude = item.PositioningInformation.Longitude;

                Location cameraLocation = new Location(latitude, longitude);
                VideoOSTreeViewItem videoOSTreeView = new VideoOSTreeViewItem();

                var customIcon = new BitmapImage(new Uri("/icon/icon.png", UriKind.Relative));
                var marker = new Image
                {
                    Source = customIcon,
                    Width = 30,
                    Height = 30
                };

                MapLayer.SetPosition(marker, cameraLocation);
                mapControl.Children.Add(marker);

                mapControl.Center = cameraLocation;
                mapControl.ZoomLevel = 16;


                GetLongitudeAndLatitude(item, videoOSTreeView); 
            }
            else
            {
                MessageBox.Show("Kaamera laius- ja pikkuskraade ei leitud või need on puudulikud.", "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Kaamera list
        private void SetupImageViewer()
        {
            if (_selectItem != null)
            {
                _imageViewerWpfControl.CameraFQID = _selectItem.FQID;
                _imageViewerWpfControl.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
                _imageViewerWpfControl.EnableVisibleLiveIndicator = EnvironmentManager.Instance.Mode == Mode.ClientLive;
                _imageViewerWpfControl.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;
                _imageViewerWpfControl.Initialize();
                _imageViewerWpfControl.Connect();
                _imageViewerWpfControl.Selected = true;
                _imageViewerWpfControl.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
            }
            else
            {
                MessageBox.Show("Valitud kaamera andmed puuduvad.", "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ImageViewerWpfControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _imageViewerWpfControl.Selected = true;
        }
        void ImageOrPaintChangedHandler(object sender, EventArgs e)
        {
            Debug.WriteLine("ImageSize:" + _imageViewerWpfControl.ImageSize.Width + "x" + _imageViewerWpfControl.ImageSize.Height + ", PaintSIze:" +
            _imageViewerWpfControl.PaintSize.Width + "x" + _imageViewerWpfControl.PaintSize.Height +
            ", PaintLocation:" + _imageViewerWpfControl.PaintLocation.X + "-" + _imageViewerWpfControl.PaintLocation.Y);
        }
        private void ButtonStartRecording_Click(object sender, RoutedEventArgs e)
        {
            if (_selectItem != null)
                EnvironmentManager.Instance.PostMessage(
                    new Message(MessageId.Control.StartRecordingCommand), _selectItem.FQID);
        }
        private void ButtonStopRecording_Click(object sender, RoutedEventArgs e)
        {
            if (_selectItem != null)
                EnvironmentManager.Instance.PostMessage(
                    new Message(MessageId.Control.StopRecordingCommand), _selectItem.FQID);
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
        //private async Task DelayToDatabaseAsync()
        //{
        //    await Task.Delay(2000);

        //}

        private void ButtonMode_Click(object sender, RoutedEventArgs e)
        {
            if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
            {
                EnvironmentManager.Instance.Mode = Mode.ClientPlayback;
                _buttonMode1.Content = "Praegune režiim: Taasesitus";
            }
            else
            {
                EnvironmentManager.Instance.Mode = Mode.ClientLive;
                _buttonMode1.Content = "Praegune režiim: Otse esitlus";
            }
            _buttonReverse.IsEnabled = EnvironmentManager.Instance.Mode == Mode.ClientPlayback;
            _buttonForward.IsEnabled = EnvironmentManager.Instance.Mode == Mode.ClientPlayback;
            _buttonStop.IsEnabled = EnvironmentManager.Instance.Mode == Mode.ClientPlayback;
        }

        private void ButtonReverse_Click(object sender, RoutedEventArgs e)
        {
            if (_speed == 0.0)
                _speed = 1.0;
            else
                _speed *= 2;
            EnvironmentManager.Instance.SendMessage(new Message(
                MessageId.SmartClient.PlaybackCommand,
                new PlaybackCommandData() { Command = PlaybackData.PlayReverse, Speed = _speed }));
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new Message(
            MessageId.SmartClient.PlaybackCommand,
            new PlaybackCommandData() { Command = PlaybackData.PlayStop }));
            EnvironmentManager.Instance.Mode = Mode.ClientPlayback;
            _buttonMode1.Content = "Praegune režiim: Taasesitus";
            _speed = 0.0;
        }

        private void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            if (_speed == 0.0)
                _speed = 1.0;
            else
                _speed *= 2;
            EnvironmentManager.Instance.SendMessage(new Message(
                MessageId.SmartClient.PlaybackCommand,
                new PlaybackCommandData() { Command = PlaybackData.PlayForward, Speed = _speed }));
        }

        private object PlaybackTimeChangedHandler(Message message, FQID dest, FQID sender)
        {
            Dispatcher.Invoke(() =>
            {
                DateTime time = (DateTime)message.Data;
                
                if (sender == null)
                    _textBoxTime1.Text = time.ToShortDateString() + "  " + time.ToLongTimeString();
            });
            return null;
        }
    }
}