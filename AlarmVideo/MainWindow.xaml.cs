using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using VideoOS.Platform.UI.Controls;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Location = Microsoft.Maps.MapControl.WPF.Location;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Proxy.AlarmClient;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow
    {

        private Item _selectItem1;
        private AlarmClientManager _alarmClientManager;
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

            // Ensure to call LoadClientAlarmsToListBox after initializing _alarmClientManager
            LoadClientAlarmsToListBox();
        }

        private void LoadClientAlarmsToListBox()
        {
            try
            {
                IAlarmClient alarmClient = _alarmClientManager.GetAlarmClient(EnvironmentManager.Instance.MasterSite.ServerId);
                AlarmLine[] alarms = alarmClient.GetAlarmLines(1, 100, new AlarmFilter()
                {
                    Orders = new OrderBy[] { new OrderBy() { Order = Order.Descending, Target = Target.Timestamp } }
                });

                foreach (AlarmLine line in alarms)
                {
                    // Fetch the Milestone SDK Alarm based on its identifier
                    VideoOS.Platform.Data.Alarm milestoneAlarm = alarmClient.Get(line.Id);

                    // Ensure that the Milestone SDK alarm is not null before accessing its properties
                    if (milestoneAlarm != null)
                    {
                        // Create and add a new instance of your Alarm class
                        alarmsListBox.Items.Add(new Alarm
                        {
                            Time = milestoneAlarm.EventHeader.Timestamp.ToLocalTime(), // Adjust property name accordingly
                            Camera = milestoneAlarm.EventHeader.Source.Name, // Adjust property name accordingly
                                                                             //Priority = milestoneAlarm.EventHeader.Priority // Adjust property name accordingly
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("LoadClientAlarmsToListBox", ex);
            }
        }





        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Alarm newAlarm = new Alarm
            //{
            //    Time = DateTime.Now,
            //    Camera = "Võru kaamera 4",
            //    Priority = "1 "
            //};

            //alarmsListBox.Items.Add(newAlarm);
        }

        private void acceptAlarmsButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void EventAlarmsButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void WrongAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void AlarmRequestButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void SendVideoButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void AlarmClosedButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void ActiveAlarms_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void WorkAlarms_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void ClosedAlarms_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void MyAlarms_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private void AllAlarms_Click(object sender, RoutedEventArgs e)
        {
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }

        private double GetLatitudeFromGisPoint(string gisPoint)
        {
            // Example format: "POINT (LONGITUDE LATITUDE)"
            // Split the string and extract the latitude
            string[] coordinates = gisPoint.Replace("POINT", "").Replace("(", "").Replace(")", "").Split(' ');

            if (coordinates.Length >= 2 && double.TryParse(coordinates[1], out double latitude))
            {
                return latitude;
            }

            return 0.0; // Default value if latitude cannot be obtained
        }

        private double GetLongitudeFromGisPoint(string gisPoint)
        {
            // Example format: "POINT (LONGITUDE LATITUDE)"
            // Split the string and extract the longitude
            string[] coordinates = gisPoint.Replace("POINT", "").Replace("(", "").Replace(")", "").Split(' ');

            if (coordinates.Length >= 2 && double.TryParse(coordinates[0], out double longitude))
            {
                return longitude;
            }

            return 0.0; // Default value if longitude cannot be obtained
        }

        private double GetLatitudeFromSDK(Item cameraItem)
        {
            if (cameraItem != null && cameraItem.Properties != null)
            {
                // Try to get latitude from "Latitude" property
                if (cameraItem.Properties.TryGetValue("Latitude", out var latitudeValue) &&
                    double.TryParse(latitudeValue.ToString(), out var latitude))
                {
                    return latitude;
                }

                // Additional checks or methods for latitude retrieval can be added here

                // Fallback: Return 0.0 if latitude cannot be obtained
            }

            return 0.0; // Default value if latitude cannot be obtained
        }

        private double GetLongitudeFromSDK(Item cameraItem)
        {
            if (cameraItem != null && cameraItem.Properties != null)
            {
                // Try to get longitude from "Longitude" property
                if (cameraItem.Properties.TryGetValue("Longitude", out var longitudeValue) &&
                    double.TryParse(longitudeValue.ToString(), out var longitude))
                {
                    return longitude;
                }

                // Additional checks or methods for longitude retrieval can be added here

                // Fallback: Return 0.0 if longitude cannot be obtained
            }

            return 0.0; // Default value if longitude cannot be obtained
        }


        // Parse GisPoint string and extract latitude or longitude based on index
        private string ParseGisPoint(string gisPoint, int index)
        {
            // Example GisPoint format: "POINT (LONGITUDE LATITUDE)"
            var coordinates = gisPoint.Replace("POINT (", "").Replace(")", "").Split(' ');
            return coordinates.Length > index ? coordinates[index] : "";
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
            // Create a new alarm object with the desired properties
            Alarm newAlarm = new Alarm
            {
                //AlarmType = "New Alarm",
                //TimeStamp = DateTime.Now
            };

            // Add the new alarm to the list of alarms
            alarmsListBox.Items.Add(newAlarm);
        }
    }
}