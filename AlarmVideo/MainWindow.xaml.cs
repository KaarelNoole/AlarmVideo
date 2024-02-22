using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using VideoOS.Platform.UI.Controls;
//using Milestone.XProtect.Client.UI.Maps;
//using Milestone.XProtect.Client.UI.Maps.MapService;
using VideoOS.Platform.ConfigurationItems;
using MapControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;
using Microsoft.Maps.MapControl.WPF;
using GMap.NET.MapProviders;
using System.Net;
//using System.Device.Location;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow
    {

        private Item _selectItem1;
        public MainWindow()
        {
            InitializeComponent();
            // Set the initial coordinates for the camera
            double initialLatitude = 59.310565; // Example latitude
            double initialLongitude = 24.429899; // Example longitude
            double initialZoomLevel = 18; // Example zoom level

            //// Set the camera position
            Microsoft.Maps.MapControl.WPF.Location initialLocation = new Microsoft.Maps.MapControl.WPF.Location(initialLatitude, initialLongitude);
            mapControl.Center = initialLocation;
            mapControl.ZoomLevel = initialZoomLevel;
        }
        //private void InitializeMap()
        //{

        //    gmapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
        //    GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
        //    gmapControl.Position = new GMap.NET.PointLatLng(0, 0);
        //    gmapControl.Zoom = 13;
        //}

        //public void ConnectToServer()
        //{
        //    // Connect to the Milestone Management Server
        //    MIP.Initialize();
        //    MIP.Server.Connect("serverAddress", "username", "password");

        //    // Retrieve camera information
        //    Camera[] cameras = MIP.System.GetCameras();

        //    // Display camera coordinates on the map
        //    foreach (var camera in cameras)
        //    {
        //        double latitude = camera.Latitude;
        //        double longitude = camera.Longitude;

        //        // Add code to display markers on the map using latitude and longitude
        //    }

        //    // Set camera coordinates (this is a simplified example, actual implementation may vary)
        //    Camera targetCamera = cameras[0];
        //    targetCamera.Latitude = newLatitude;
        //    targetCamera.Longitude = newLongitude;
        //    targetCamera.Update();
        //}

        //private void AddMarkerToMap(double latitude, double longitude, string title)
        //{
        //    TextAnnotation textAnnotation = new TextAnnotation
        //    {
        //        Location = new Geopoint(new BasicGeoposition
        //        {
        //            Latitude = latitude,
        //            Longitude = longitude
        //        }),
        //        Text = title
        //    };

        //    // Add the text annotation to the map
        //    mapView.Children.Add(textAnnotation);
        //}

        private void AddButton_Click(object sender, RoutedEventArgs e)
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

            //if (_selectItem1 != null)
            //{
            //    // Update the map with the camera location
            //    //double cameraLatitude = _selectItem1.Position.Latitude;
            //    //double cameraLongitude = _selectItem1.Position.Longitude;

            //    // Set the camera location on the map
            //    mapControl.Center = new Microsoft.Maps.MapControl.WPF.Location(cameraLatitude, cameraLongitude);

            //    // Optionally, you can set a marker or other visual representation on the map
            //    // based on the camera location.
            //    // For example, you might use a Pushpin from the Bing Maps control.
            //    var pushpin = new Microsoft.Maps.MapControl.WPF.Pushpin();
            //    pushpin.Location = new Microsoft.Maps.MapControl.WPF.Location(cameraLatitude, cameraLongitude);
            //    mapControl.Children.Add(pushpin);
            //}

            if (itemPicker.ShowDialog().Value)
            {
                _selectItem1 = itemPicker.SelectedItems.First();
                buttonSelect1.Content = _selectItem1.Name;

                _imageViewerWpfControl.CameraFQID = _selectItem1.FQID;
                // Lets enable/disable the header based on the tick mark. Could also disable LiveIndicator or CameraName.
                _imageViewerWpfControl.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
                _imageViewerWpfControl.EnableVisibleLiveIndicator = EnvironmentManager.Instance.Mode == Mode.ClientLive;
                _imageViewerWpfControl.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;

                _imageViewerWpfControl.Initialize();
                _imageViewerWpfControl.Connect();
                _imageViewerWpfControl.Selected = true;

                _imageViewerWpfControl.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
            }
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

        private void alarmDetailsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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

       

        //private async void InitializeMap()
        //{
        //    // Connect to the Milestone Map Service
        //    MapServiceClient mapServiceClient = new MapServiceClient();
        //    await mapServiceClient.ConnectAsync();

        //    // Set the map service as the geographic background for the Map control
        //    MapView.MapService = mapServiceClient;

        //    // Retrieve the camera information and coordinates from the Milestone management client
        //    List<Camera> cameras = await GetCamerasAsync();

        //    // Add camera icons to the map at the correct location
        //    foreach (Camera camera in cameras)
        //    {
        //        MapIcon mapIcon = new MapIcon
        //        {
        //            Location = new Location(camera.Latitude, camera.Longitude),
        //            Title = camera.Name,
        //            Description = camera.Description
        //        };

        //        CameraIcons.Items.Add(mapIcon);
        //    }
        //}

        //private async Task<List<Camera>> GetCamerasAsync()
        //{
        //    // Replace this with your actual code to retrieve the camera information and coordinates from the Milestone management client
        //    List<Camera> cameras = new List<Camera>
        //    {
        //        new Camera { Name = "Camera 1", Description = "Description 1", Latitude = 52.370216, Longitude = 4.895168 },
        //        new Camera { Name = "Camera 2", Description = "Description 2", Latitude = 52.370216, Longitude = 4.895168 }
        //    };

        //    return cameras;
        //}


    }
}