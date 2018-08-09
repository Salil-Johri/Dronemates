using DroneApp.DataBase;
using DroneApp.PhoneDialer;
using DroneApp.TKMap.Pages;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.FlightPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SitePrepPage : ContentPage
    {
        public SitePrepPage()
        {
            InitializeComponent();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
            await Navigation.PopAsync();
        }

        private async void OnViewMap(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
            if (answer)
            {
                var curr_appt = await App.Database.GetItemAsync(((Appointment)BindingContext).ID);
                var appt = await App.Database.database.GetWithChildrenAsync<Appointment>(curr_appt.ID);
                await Navigation.PushAsync(new SamplePage(appt.Longitude, appt.Latitude, appt));
            }
        }
        private async void OnAerodromeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AerodromePage() { BindingContext = (Appointment)BindingContext });
        }
        private async void UnitConvert(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
            var answer = await DisplayAlert("Are you sure you want to convert this?", null, "Yes", "No");
            if (answer)
            {
                if ((sender as Button).Text == "To Meter")
                {
                    double feet = ((Appointment)BindingContext).MaxAlt;
                    double meter = feet * 0.31;
                    ((Appointment)BindingContext).MaxAlt = meter;
                    MaxAltEntry.Text = meter.ToString();
                }
                else
                {
                    double meter = ((Appointment)BindingContext).MaxAlt;
                    double feet = meter / 0.31;
                    ((Appointment)BindingContext).MaxAlt = feet;
                    MaxAltEntry.Text = feet.ToString();
                }
            }
        }
        private void OnDroneType(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Inspire 1")
            {
                ((Appointment)BindingContext).UAVType = "DJI Inspire 1";
                ((Appointment)BindingContext).Wingspan = "45cm";
                ((Appointment)BindingContext).UAVColour = "White";
                ((Appointment)BindingContext).UAVWeight = "6.74";
                WingspanEntry.Text = "45cm";
                ColourEntry.Text = "White";
            }
            else
            {
                ((Appointment)BindingContext).UAVType = "DJI Mavic Air";
                ((Appointment)BindingContext).Wingspan = "213mm";
                ((Appointment)BindingContext).UAVColour = "Grey";
                ((Appointment)BindingContext).UAVWeight = "425.243";
                WingspanEntry.Text = "213mm";
                ColourEntry.Text = "Grey";
            }
        }
        private async void OnCallClicked(object sender, EventArgs e)
        {
            string CallNumber = null;
            if ((sender as Button).Text == "Call Contact")
            {
                CallNumber = ((Appointment)BindingContext).ContactNum;
            }
            else
            {
                CallNumber = ((Appointment)BindingContext).NotamNum;
            }

            if (CallNumber != null)
            {
                if (await this.DisplayAlert(
                       "Dial a Number",
                       "Would you like to call " + CallNumber + "?",
                       "Yes",
                       "No"))
                {
                    var dialer = DependencyService.Get<IDialer>();
                    if (dialer != null)
                        dialer.Dial(CallNumber);
                }
            }
            else
                await DisplayAlert("Error", "There's no number provided", "Ok");
        }
        private async void OnACCManagerCall(object sender, EventArgs e)
        {
            string CallNumber = ((Appointment)BindingContext).ACCNumber;

            if (CallNumber != null)
            {
                if (await this.DisplayAlert(
                       "Dial a Number",
                       "Would you like to call " + CallNumber + "?",
                       "Yes",
                       "No"))
                {
                    ((Appointment)BindingContext).AccCalled = "Yes";
                    var dialer = DependencyService.Get<IDialer>();
                    if (dialer != null)
                        dialer.Dial(CallNumber);
                }
            }
            else
                await DisplayAlert("Error", "Select an ACC Shift Manager", "Ok");
        }
        private void ACCSelected(object sender, EventArgs e)
        {
            if (sitePrepQuestion_7.SelectedIndex == 0)
            {
                ((Appointment)BindingContext).ACCNumber = "17808908397";
            }
            else if (sitePrepQuestion_7.SelectedIndex == 1)
            {
                ((Appointment)BindingContext).ACCNumber = "17096515207";
            }
            else if (sitePrepQuestion_7.SelectedIndex == 2)
            {
                ((Appointment)BindingContext).ACCNumber = "15068677173";
            }
            else if (sitePrepQuestion_7.SelectedIndex == 3)
            {
                ((Appointment)BindingContext).ACCNumber = "15146333365";
            }
            else if (sitePrepQuestion_7.SelectedIndex == 4)
            {
                ((Appointment)BindingContext).ACCNumber = "19056764509";
            }
            else if (sitePrepQuestion_7.SelectedIndex == 5)
            {
                ((Appointment)BindingContext).ACCNumber = "16045864500";
            }
            else
            {
                ((Appointment)BindingContext).ACCNumber = "12049838338";
            }
        }
    }
}