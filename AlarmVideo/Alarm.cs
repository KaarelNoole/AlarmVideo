using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AlarmVideo
{
    public class Alarm : INotifyPropertyChanged
    {

        public int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private DateTime _eventTime;
        public DateTime EventTime
        {
            get { return _eventTime; }
            set
            {
                _eventTime = value;
                OnPropertyChanged(nameof(EventTime));
            }
        }
        public string _source { get; set; }
        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }
        public string _event { get; set; }
        public string Event
        {
            get { return _event; }
            set
            {
                _event = value;
                OnPropertyChanged(nameof(Event));
            }
        }

        public string _status { get; set; }
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}