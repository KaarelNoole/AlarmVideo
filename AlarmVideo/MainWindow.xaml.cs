using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using VideoOS.Platform.UI.Controls;
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MapControl;
using Location = Microsoft.Maps.MapControl.WPF.Location;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow
    {

        private Item _selectItem1;
        public MainWindow()
        {
            InitializeComponent();

            double initialLatitude = 58.883333;
            double initialLongitude = 25.557222;
            double initialZoomLevel = 6;
            Location initialLocation = new Location(initialLatitude, initialLongitude);

            mapControl.Center = initialLocation;
            mapControl.ZoomLevel = initialZoomLevel;

        }

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


            if (itemPicker.ShowDialog().Value)
            {

                //double initialLatitude = _selectItem1.Latitude ?? 0;
                //double initialLongitude = _selectItem1.Longitude ?? 0; 
                //double initialZoomLevel = 16;
                double initialLatitude = 59.310565;
                double initialLongitude = 24.429899;
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


                _selectItem1 = itemPicker.SelectedItems.First();
                    buttonSelect1.Content = _selectItem1.Name;
                    _imageViewerWpfControl.CameraFQID = _selectItem1.FQID;
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