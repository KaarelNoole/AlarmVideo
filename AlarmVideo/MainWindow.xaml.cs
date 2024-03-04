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
using VideoOS.Platform.Client;
using System.Windows.Forms;
using VideoOS.Platform.Data;

namespace AlarmVideo
{
    public partial class MainWindow : VideoOSWindow
    {
        private object _obj1;
        private Item _selectItem1;
        private AlarmClientManager _alarmClientManager;
        private MessageCommunication _messageCommunication;

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

            LoadClientAlarmsToListBox();
        }

        //private void subscribeAlarms()
        //{
        //    if (_obj1 == null)
        //    {
        //        _obj1 = _messageCommunication.RegisterCommunicationFilter(NewAlarmMessageHandler,
        //           new CommunicationIdFilter(MessageId.Server.NewAlarmIndication), null, EndPointType.Server);
        //    }
        //}

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
                    VideoOS.Platform.Data.Alarm milestoneAlarm = alarmClient.Get(line.Id);

                    if (milestoneAlarm != null)
                    {
                        alarmsListBox.Items.Add(new Alarm
                        {
                            Time = milestoneAlarm.EventHeader.Timestamp.ToLocalTime(),
                            Camera = milestoneAlarm.EventHeader.Source.Name,
                            //Priority = milestoneAlarm.EventHeader.Priority,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("LoadClientAlarmsToListBox", ex);
            }
        }

        private void NewAlarmMessageHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            Alarm alarm = message.Data as Alarm;

            if (alarm != null)
            {
                if (Dispatcher.CheckAccess())
                {
                    // Execute on the UI thread directly
                    ProcessNewAlarm(alarm);
                }
                else
                {
                    // Execute on the UI thread using Dispatcher
                    Dispatcher.Invoke(() => ProcessNewAlarm(alarm));
                }
            }

        }

        private void ProcessNewAlarm(Alarm alarm)
        {

                LoadClientAlarmsToListBox();
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

        //private void Button_Select1_Click(object sender, RoutedEventArgs e)
        //{
        //    _imageViewerWpfControl.Disconnect();
        //    _imageViewerWpfControl.Close();

        //    ItemPickerWpfWindow itemPicker = new ItemPickerWpfWindow()
        //    {
        //        KindsFilter = new List<Guid> { Kind.Camera },
        //        SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
        //        Items = Configuration.Instance.GetItems()
        //    };

        //    if (itemPicker.ShowDialog().Value)
        //    {
        //        _selectItem1 = itemPicker.SelectedItems.First();

        //        // Check for null
        //        if (_selectItem1 != null)
        //        {
        //            SetupMapAndMarker();
        //            SetupImageViewer();
        //        }
        //    }
        //}

        //private void SetupMapAndMarker()
        //{
        //    double initialLatitude = GetLatitudeFromSDK(_selectItem1);
        //    double initialLongitude = GetLongitudeFromSDK(_selectItem1);
        //    double initialZoomLevel = 16;

        //    // Set the initial camera position
        //    Location initialLocation = new Location(initialLatitude, initialLongitude);

        //    // Set the map control properties
        //    mapControl.Center = initialLocation;
        //    mapControl.ZoomLevel = initialZoomLevel;

        //    // Create a custom marker (Image)
        //    var customIcon = new BitmapImage(new Uri("/icon/icon.png", UriKind.Relative));

        //    var marker = new Image
        //    {
        //        Source = customIcon,
        //        Width = 30,
        //        Height = 30
        //    };

        //    // Set the location for the marker on the map
        //    MapLayer.SetPosition(marker, initialLocation);

        //    // Add the marker to the map's children
        //    mapControl.Children.Add(marker);

        //    buttonSelect1.Content = _selectItem1.Name;
        //}

        //private void SetupImageViewer()
        //{
        //    _imageViewerWpfControl.CameraFQID = _selectItem1.FQID;
        //    _imageViewerWpfControl.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
        //    _imageViewerWpfControl.EnableVisibleLiveIndicator = EnvironmentManager.Instance.Mode == Mode.ClientLive;
        //    _imageViewerWpfControl.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;
        //    _imageViewerWpfControl.Initialize();
        //    _imageViewerWpfControl.Connect();
        //    _imageViewerWpfControl.Selected = true;
        //    _imageViewerWpfControl.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
        //}


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

        //private void checkBoxHeader_Checked(object sender, RoutedEventArgs e)
        //{
        //    UpdateCheckBoxHeader();
        //}

        //private void CheckBoxHeader_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    UpdateCheckBoxHeader();
        //}

        //private void checkBoxDigitalZoom_Checked(object sender, RoutedEventArgs e)
        //{
        //    UpdateCheckBoxDigitalZoom();
        //}

        //private void CheckBoxDigitalZoom_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    UpdateCheckBoxDigitalZoom();
        //}

        //private void checkBoxAdaptiveStreaming_Checked(object sender, RoutedEventArgs e)
        //{
        //    UpdateCheckBoxAdaptiveStreaming();
        //}

        //private void CheckBoxAdaptiveStreaming_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    UpdateCheckBoxAdaptiveStreaming();
        //}

        //private void UpdateCheckBoxHeader()
        //{
        //    _imageViewerWpfControl.EnableVisibleHeader = checkBoxHeader.IsChecked.Value;
        //}

        //private void UpdateCheckBoxDigitalZoom()
        //{
        //    _imageViewerWpfControl.EnableDigitalZoom = checkBoxDigitalZoom.IsChecked.Value;
        //}

        //private void UpdateCheckBoxAdaptiveStreaming()
        //{
        //    _imageViewerWpfControl.AdaptiveStreaming = checkBoxAdaptiveStreaming.IsChecked.Value;
        //}

        private void alarmDetailsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}