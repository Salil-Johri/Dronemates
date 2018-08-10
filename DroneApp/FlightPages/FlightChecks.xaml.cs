using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
using DroneApp.TKMap.Pages;
using DroneApp.PhoneDialer;
using SQLiteNetExtensionsAsync.Extensions;

namespace DroneApp.FlightPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightChecks : TabbedPage
    {
        public FlightChecks() => InitializeComponent();
      
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NotamDatePickerStart.MinimumDate = DateTime.Now;
            NotamDatePickerEnd.MinimumDate = DateTime.Now; 
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
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
        }
        private async void UnitConvertAsync(object sender, EventArgs e)
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

        private async void UnitConvertAsyncRadius(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
            var answer = await DisplayAlert("Are you sure you want to convert this?", null, "Yes", "No");
            if (answer)
            {
                if ((sender as Button).Text == "To Meter")
                {
                    double feet = ((Appointment)BindingContext).Radius;
                    double meter = feet * 0.31;
                    ((Appointment)BindingContext).Radius = meter;
                    radiusQuestion3.Text = meter.ToString();
                }
                else
                {
                    double meter = ((Appointment)BindingContext).Radius;
                    double feet = meter / 0.31;
                    ((Appointment)BindingContext).Radius = feet;
                    radiusQuestion3.Text = feet.ToString();
                }
            }
        }
        private void OnDroneTypeClicked(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Inspire 1")
            {
                ((Appointment)BindingContext).UAVType = "DJI Inspire 1";
                ((Appointment)BindingContext).Wingspan = "45cm";
                ((Appointment)BindingContext).UAVColour = "White";
                ((Appointment)BindingContext).UAVWeight = "7.49";
                WingspanEntry.Text = "45cm";
                ColourEntry.Text = "White";
                WeightEntry.Text = "7.49";
            }
            else
            {
                ((Appointment)BindingContext).UAVType = "DJI Mavic Air";
                ((Appointment)BindingContext).Wingspan = "213mm";
                ((Appointment)BindingContext).UAVColour = "Grey";
                ((Appointment)BindingContext).UAVWeight = "0.94";
                WingspanEntry.Text = "213mm";
                ColourEntry.Text = "Grey";
                WeightEntry.Text = "0.94";
            }
        }
        private async void OnCallClickedAsync(object sender, EventArgs e)
        {
            string CallNumber = ((Appointment)BindingContext).NotamNum;

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
            {
                await DisplayAlert("Error", "There's no number provided", "Ok");
            }
        }
        private async void OnACCManagerCallAsync(object sender, EventArgs e)
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
        private void OnWeatherClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(uriString: "https://flightplanning.navcanada.ca/cgi-bin/CreePage.pl?Langue=anglais&NoSession=NS_Inconnu&Page=Fore-obs%2Fmetar-taf-map&TypeDoc=html"));
        }
        private void ACCManagerSelected(object sender, EventArgs e)
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

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)(sitePrepQuestion_2.SelectedItem) == "Class G")
            {
                Site_PrepQ8.IsVisible = false;
            }
            else
            {
                Site_PrepQ8.IsVisible = true;
            }
        }
        private async void OnAreaClicked(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);

            if ((sender as Button).Text == "To Meters")
            {
                double feet = ((Appointment)BindingContext).MaxAlt;
                double meter = feet * 0.31;
                ((Appointment)BindingContext).MaxAlt = meter;
                AltNotam.Text = meter.ToString();
            }
            else
            {
                double meter = ((Appointment)BindingContext).MaxAlt;
                double feet = meter / 0.31;
                ((Appointment)BindingContext).MaxAlt = feet;
                AltNotam.Text = feet.ToString();
            }
        }
        private async void OnRadiusAreaClicked(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);

            if ((sender as Button).Text == "To Meters")
            {
                double feet = ((Appointment)BindingContext).RadiusNM;
                double meter = feet * 1852;
                ((Appointment)BindingContext).RadiusNM = meter;
                RadiusNotam.Text = meter.ToString();
            }
            else
            {
                double meter = ((Appointment)BindingContext).RadiusNM;
                double feet = meter / 1852;
                ((Appointment)BindingContext).RadiusNM = feet;
                RadiusNotam.Text = feet.ToString();
            }
        }
    
    }
}