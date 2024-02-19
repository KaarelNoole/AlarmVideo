using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using VideoOS.Platform.UI.Controls;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow
    {

        private Item _selectItem1;
        public MainWindow()
        {
            InitializeComponent();
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
            _imageViewerWpfControl1.Disconnect();
            _imageViewerWpfControl1.Close();

            ItemPickerWpfWindow itemPicker = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid> { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                Items = Configuration.Instance.GetItems()
            };

            if (itemPicker.ShowDialog().Value)
            {
                _selectItem1 = itemPicker.SelectedItems.First();
                buttonSelect1.Content = _selectItem1.Name;

                _imageViewerWpfControl1.CameraFQID = _selectItem1.FQID;
                // Lets enable/disable the header based on the tick mark. Could also disable LiveIndicator or CameraName.
                _imageViewerWpfControl1.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
                _imageViewerWpfControl1.EnableVisibleLiveIndicator = EnvironmentManager.Instance.Mode == Mode.ClientLive;
                _imageViewerWpfControl1.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;

                _imageViewerWpfControl1.Initialize();
                _imageViewerWpfControl1.Connect();
                _imageViewerWpfControl1.Selected = true;

                _imageViewerWpfControl1.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
            }
        }

        private void ImageViewerWpfControl1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _imageViewerWpfControl1.Selected = true;
        }

        void ImageOrPaintChangedHandler(object sender, EventArgs e)
        {
            Debug.WriteLine("ImageSize:" + _imageViewerWpfControl1.ImageSize.Width + "x" + _imageViewerWpfControl1.ImageSize.Height + ", PaintSIze:" +
                            _imageViewerWpfControl1.PaintSize.Width + "x" + _imageViewerWpfControl1.PaintSize.Height +
                            ", PaintLocation:" + _imageViewerWpfControl1.PaintLocation.X + "-" + _imageViewerWpfControl1.PaintLocation.Y);
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
            _imageViewerWpfControl1.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
        }

        private void UpdateCheckBoxDigitalZoom()
        {
            _imageViewerWpfControl1.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
        }

        private void UpdateCheckBoxAdaptiveStreaming()
        {
            _imageViewerWpfControl1.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;
        }

    }
}