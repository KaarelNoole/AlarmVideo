using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;
using System.Windows.Controls;
using Location = Microsoft.Maps.MapControl.WPF.Location;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Proxy.AlarmClient;
using System.Windows.Forms;
using VideoOS.Platform.Data;
using Microsoft.Maps.MapControl.WPF;
using System.Collections.Generic;
using VideoOS.Platform.UI;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MessageBox = System.Windows.MessageBox;
using System.Data.SqlClient;
using VideoOS.Platform.EventsAndState;
using System.Windows.Threading;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow
    {
        private object _obj1;
        private Item _selectItem1;
        private AlarmClientManager _alarmClientManager;
        DataGridViewRow _selectedRow = null;
        private MessageCommunication _messageCommunication;
        private List<Alarm> _alarms;
        private IAlarmClient alarmClient;

        private ObservableCollection<Alarm> alarmsCollection = new ObservableCollection<Alarm>();
        private DispatcherTimer timer;




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
            timer.Interval = TimeSpan.FromSeconds(30); // Set the interval for checking (e.g., every 30 seconds)
            timer.Tick += Timer_Tick;
            timer.Start();

            LoadClientAlarmsToListBox();
            //SubscribeAlarms();
            _alarms = new List<Alarm>();
            


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Query the database for new alarms
            LoadClientAlarmsToListBox();
        }

        //private void InitializeMessageCommunication()
        //{
        //    _messageCommunication = new MessageCommunication();
        //    _messageCommunication.Init(new ServerId(EnvironmentType.Service.ToString(), "", 80, Guid.Empty));
        //}

        //private void SubscribeAlarms()
        //{
        //    _obj1 = _messageCommunication.RegisterCommunicationFilter(OnEventsReceived,
        //        new CommunicationIdFilter(MessageId.Server.NewAlarmIndication), null, EndPointType.Server);
        //}

        //private void SubscribeToEvents()
        //{
        //    // Subscribe to the event source to receive events
        //    // Replace YourEventSource with the actual event source you're using
        //    eventSource.EventsReceived += OnEventsReceived;
        //}

        //private void UnsubscribeFromEvents()
        //{
        //    // Unsubscribe from the event source when no longer needed
        //    // Replace YourEventSource with the actual event source you're using
        //    YourEventSource.EventsReceived -= OnEventsReceived;
        //}


        //private void OnEventsReceived(object sender, IEnumerable<Event> events)
        //{
        //    // Update the alarmsListBox
        //    LoadClientAlarmsToListBox();
        //}


        private void LoadClientAlarmsToListBox()
        {
            try
            {
                // Connect to the database
                string connectionString = "Data Source=10.100.80.67;Initial Catalog=minubaas;User ID=minunimi;Password=test;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Query to select data from the Camera table
                    string query = "SELECT EventTime, Source, Event FROM Camera";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Clear existing items in the ListBox
                            alarmsListBox.Items.Clear();
                            // Read data and populate ListBox
                            while (reader.Read())
                            {
                                // Ensure the data type retrieved matches the EventTime property
                                if (!reader.IsDBNull(0)) // Check for null values
                                {
                                    // If EventTime column is not null, retrieve it as DateTime
                                    DateTime eventTime = reader.GetDateTime(0);
                                    // Get other values from the reader
                                    string source = reader.GetString(1);
                                    string eventType = reader.GetString(2);

                                    // Create Alarm object and add to ListBox
                                    alarmsListBox.Items.Add(new Alarm
                                    {
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
                EnvironmentManager.Instance.ExceptionDialog("LoadClientAlarmsToListBox", ex);
            }
        }

        private IEnumerable<Alarm> ExtractAlarmsFromEvents(IEnumerable<Event> events)
        {
            List<Alarm> newAlarms = new List<Alarm>();

            foreach (var @event in events)
            {
                // Extract necessary information from the event to create an Alarm object
                DateTime eventTime = @event.Time;
                string source = @event.Source;
                string eventType = @event.Type.ToString(); // or any property that corresponds to the event description

                // Create a new Alarm object
                Alarm alarm = new Alarm
                {
                    EventTime = eventTime,
                    Source = source,
                    Event = eventType
                };

                // Add the new alarm to the list
                newAlarms.Add(alarm);
            }

            return newAlarms;
        }

        private void NewAlarmMessageHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
            {
                Alarm alarm = message.Data as Alarm;
                if (alarm != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        // Assuming your Alarm class has Timestamp, Source, and Priority properties
                        string content = $"{alarm.EventTime.ToLocalTime()}, {alarm.Source}, {alarm.Event}";
                        ListBoxItem listBoxItem = new ListBoxItem
                        {
                            Content = content
                        };

                        // Assuming alarmsListBox is the name of your ListBox
                        alarmsListBox.Items.Insert(0, listBoxItem);
                    });
                }
            }
        private void AlarmListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item from the ListBox
            Alarm selectedAlarm = alarmsListBox.SelectedItem as Alarm;

            // Do something with the selected alarm, for example, display its details or perform some action
            if (selectedAlarm != null)
            {
                // Do something with the selected alarm
                MessageBox.Show($"Selected Alarm: {selectedAlarm.EventTime}, {selectedAlarm.Source}, {selectedAlarm.Event}");
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void acceptAlarmsButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void EventAlarmsButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void WrongAlarmButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void AlarmRequestButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SendVideoButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AlarmClosedButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void ActiveAlarms_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void WorkAlarms_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void ClosedAlarms_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void MyAlarms_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AllAlarms_Click(object sender, RoutedEventArgs e)
        {
           
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

                // Check for null
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

            // Set the initial camera position
            Location initialLocation = new Location(initialLatitude, initialLongitude);

            // Set the map control properties
            mapControl.Center = initialLocation;
            mapControl.ZoomLevel = initialZoomLevel;

            // Create a custom marker (Image)
            var customIcon = new BitmapImage(new Uri("/icon/icon.png", UriKind.Relative));

            var marker = new Image
            {
                Source = customIcon,
                Width = 30,
                Height = 30
            };

            // Set the location for the marker on the map
            MapLayer.SetPosition(marker, initialLocation);

            // Add the marker to the map's children
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
                    new VideoOS.Platform.Messaging.Message(MessageId.Control.StartRecordingCommand), _selectItem1.FQID);
        }

        private void ButtonStopRecording1_Click(object sender, RoutedEventArgs e)
        {
            if (_selectItem1 != null)
                EnvironmentManager.Instance.PostMessage(
                    new VideoOS.Platform.Messaging.Message(MessageId.Control.StopRecordingCommand), _selectItem1.FQID);
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
            
        }
    }
}