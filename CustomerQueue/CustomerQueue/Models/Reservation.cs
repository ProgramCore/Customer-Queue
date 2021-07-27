using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CustomerQueue.Models
{
    public class Reservation : INotifyPropertyChanged
    {
        public Guid ID { get; private set; } = new Guid();
        public Customer Customer { get; set; }
        public DateTime QueuedOut { get; set; }
        public CustomerResolution Resolution { get; set; }
        public int PartySize { get; set; }

        private DateTime _queuedIn;
        public DateTime QueuedIn
        {
            get => _queuedIn;
            set { _queuedIn = value; OnPropertyChanged(); }
        }

        private bool _prioritize = false;
        public bool Prioritize 
        {
            get => _prioritize; 
            set { _prioritize = value; OnPropertyChanged(); }
        }

        private DateTime _alertedTime = DateTime.MinValue;
        public DateTime AlertedTime
        {
            get => _alertedTime;
            set 
            { 
                _alertedTime = value; 
                OnPropertyChanged(); 
            }
        }

        int estTime = 0;
        public int EstimatedWaitTime 
        {
            get => estTime;
            set { estTime = value; OnPropertyChanged(); }
        }

        public void TimeTick()
        {
            OnPropertyChanged(nameof(QueuedIn));
            OnPropertyChanged(nameof(AlertedTime));
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public enum CustomerResolution
    {
        CustomerSeated,
        Cancelled
    }
}
