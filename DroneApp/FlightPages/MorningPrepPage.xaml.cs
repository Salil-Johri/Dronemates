using DroneApp.DataBase;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.FlightPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MorningPrepPage : ContentPage
	{
		public MorningPrepPage ()
		{
			InitializeComponent ();
		}
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
            await Navigation.PopAsync();
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
        }
        private void OnWeather(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(uriString: "https://flightplanning.navcanada.ca/cgi-bin/CreePage.pl?Langue=anglais&NoSession=NS_Inconnu&Page=Fore-obs%2Fmetar-taf-map&TypeDoc=html"));
        }
    }
}