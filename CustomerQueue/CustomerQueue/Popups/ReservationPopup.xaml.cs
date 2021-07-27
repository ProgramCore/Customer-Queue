using CustomerQueue.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerQueue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public Reservation Reservation { get; private set; }
        private int partySize = 2;

        public ReservationPopup()
        {
            InitializeComponent();
            NameEntry.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
        }
        
        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            { return false; }

            if(partySize <= 0)
            { return false; }

            if(!string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                var attr = new EmailAddressAttribute();

                if (!attr.IsValid(EmailEntry.Text))
                {
                    return false;
                }
            }

            return true;
        }

        private async void AddReservationBtn_Clicked(object sender, EventArgs e)
        {
            if(!Validate())
            { return; }
            

            Reservation = new Reservation()
            {
                Customer = new Customer()
                {
                    Name = NameEntry.Text,
                    Email = EmailEntry.Text
                },
                PartySize = partySize,
                QueuedIn = DateTime.Now,
                Prioritize = prioritySwitch.IsToggled
            };

            await PopupNavigation.Instance.PopAsync();
        }

        private async void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var rad = sender as RadioButton;

            if (rad.IsChecked)
            {
                int val = partySize;

                if (int.TryParse(rad.Value.ToString(), out val))
                {
                    partySize = val;
                }
                else
                {
                    const string CONFIRM = "Set";
                    const string CANCEL = "Cancel";

                    var resp = await DisplayPromptAsync("Party Size", "How many members to this party?", CONFIRM, CANCEL, "", 3, Keyboard.Numeric, "");

                    if (int.TryParse(resp, out val))
                    {
                        partySize = val;
                    }
                    else
                    {
                        partySize = -1;
                        rad.IsChecked = false;
                    }
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AddReservationBtn.Clicked += AddReservationBtn_Clicked;
            NameEntry.Focus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            AddReservationBtn.Clicked -= AddReservationBtn_Clicked;
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}