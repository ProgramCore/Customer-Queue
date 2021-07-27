using CustomerQueue.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;

namespace CustomerQueue.ViewModels
{
    class QueueVM : BaseViewModel
    {
        private const string SENDER_EMAIL = "";
        private const string SENDER_EMAIL_PW = "";
        private const int SMTP_PORT = 587;
        private const string SMTP_ADDRESS = "smtp-mail.outlook.com";

        public QueueVM()
        {
            AddCustomerCommand = new Command(async () => { await CallPopup(); });
            SeatCustomerCommand = new Command<Reservation>(SeatCustomer);
            AlertCommand = new Command<Reservation>(async (r)=> { await AlertCustomer(r); });
            PrioritizeCommand = new Command<Reservation>(PrioritizeReservation);
            CancelCustomerCommand = new Command<Reservation>(CancelReservation);
            StartTimer();
        }

        private void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(15), TimeCheck);
            TimeCheck();
        }

        private async Task AlertCustomer(Reservation reservation)
        {
            reservation.AlertedTime = DateTime.Now;

            if(string.IsNullOrWhiteSpace(reservation.Customer.Email) || SENDER_EMAIL.Length == 0 || SENDER_EMAIL_PW.Length == 0)
            { return; }

            var service = new EmailService();

            var isSuccessful = await service.SendSimpleTextEmail("You're up! Come to the counter to get seated.", "Time to eat!", reservation.Customer.Email, new System.Net.NetworkCredential(SENDER_EMAIL, SENDER_EMAIL_PW), SMTP_ADDRESS, SMTP_PORT);
        
            if(!isSuccessful)
            {
                reservation.AlertedTime = DateTime.MinValue;
            }
        }

        private async Task CallPopup()
        {
            var popup = new ReservationPopup();
            await PopupNavigation.Instance.PushAsync(popup);
            popup.Disappearing += Popup_Disappearing;
        }

        private void Popup_Disappearing(object sender, EventArgs e)
        {
            var popup = sender as ReservationPopup;
            AddCustomer(popup.Reservation);
            popup.Disappearing -= Popup_Disappearing;
        }

        private void AddCustomer(Reservation newReservation)
        {
            if(newReservation == null)
            { return; }

            if(newReservation.Prioritize)
            {
                PrioritizeReservation(newReservation);
            }
            else
            {
                Reservations.Add(newReservation);
                TimeCheck();
            }
        }

        private void SeatCustomer(Reservation reservation)
        {
            QueueOutCustomer(reservation, CustomerResolution.CustomerSeated);
            TimeCheck();
        }

        public void PrioritizeReservation(Reservation reservation)
        {
            Reservations.Remove(reservation);
            reservation.Prioritize = true;

            int pos = -1;

            foreach(Reservation current in Reservations)
            {
                if(!current.Prioritize)
                {
                    pos = Reservations.IndexOf(current);
                    break;
                }
                else if (GetWaitMins(current) < GetWaitMins(reservation))
                {
                    pos = Reservations.IndexOf(current);
                    break;
                }
            }

            if(pos >= 0)
            {
                Reservations.Insert(pos, reservation);
            }
            else
            {
                Reservations.Add(reservation);
            }

            TimeCheck();
        }

        private int GetWaitMins(Reservation res)
        {
            return Converters.Helpers.MinuteDifference(DateTime.Now, res.QueuedIn);
        }

        public void CancelReservation(Reservation reservation)
        {
            Reservations.Remove(reservation);
            TimeCheck();
        }

        private void QueueOutCustomer(Reservation reservation, CustomerResolution resolution)
        {
            if (reservation == null)
            { return; }

            reservation.QueuedOut = DateTime.Now;
            reservation.Resolution = resolution;

            Reservations.Remove(reservation);
            CompletedReservations.Insert(0, reservation);
            TimeCheck();
        }

        public bool TimeCheck()
        {
            SetEstimateWaitTimes();

            foreach (Reservation reservation in Reservations)
            {
                reservation.TimeTick();
            }

            return true;
        }

        private void SetEstimateWaitTimes()
        {
            int countComp = 0;
            int sum = 0;
            int count = 0;

            foreach (Reservation compResv in CompletedReservations)
            {
                if (CompletedReservations.IndexOf(compResv) > 2)
                { break; }

                if (CompletedReservations.IndexOf(compResv) == 0)
                {
                    var span = TimeSpan.FromTicks(DateTime.Now.Ticks - compResv.QueuedOut.Ticks);

                    if (span.TotalDays < 1)
                    {
                        sum -= (int)span.TotalMinutes;
                    }
                    else
                    {
                        sum = int.MinValue;
                        break;
                    }
                }

                sum += Converters.Helpers.MinuteDifference(compResv.QueuedOut, compResv.QueuedIn);
                countComp++;
            }

            sum = sum < 0 ? 0 : sum;

            foreach (Reservation reservation in Reservations)
            {
                if(GetWaitMins(reservation) == 0)
                { continue; }

                if (count > 3)
                { break; }

                sum += GetWaitMins(reservation);
                count++;
            }


            foreach (Reservation reservation in Reservations)
            {
                reservation.EstimatedWaitTime = CalcWaitTime(sum, countComp + count, GetWaitMins(reservation), Reservations.IndexOf(reservation) + 1);
            }

            NextEstTime = CalcWaitTime(sum, countComp + count, 0, Reservations.Count + 1);
        }

        private int CalcWaitTime(int sumMins, int itemCount, int alreadyWaited, int position)
        {
            if(itemCount == 0)
            { return 0; }

            var est = (((sumMins / itemCount) * position) - (alreadyWaited + 1)) + position;

            return est < 0 ? 0 : est;
        }

        public ObservableCollection<Reservation> Reservations { get; } = new ObservableCollection<Reservation>();
        public ObservableCollection<Reservation> CompletedReservations { get; } = new ObservableCollection<Reservation>();
        public Command AddCustomerCommand { get; set; }
        public Command SeatCustomerCommand { get; set; }
        public Command AlertCommand { get; set; }
        public Command CancelCustomerCommand { get; set; }
        public Command PrioritizeCommand { get; set; }

        private int _nextEstTime;
        public int NextEstTime
        {
            get { return _nextEstTime; }
            set { _nextEstTime = value; OnPropertyChanged(); }
        }
    }
}
