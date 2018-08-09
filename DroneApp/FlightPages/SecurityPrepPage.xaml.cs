using DroneApp.DataBase;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.FlightPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecurityPrepPage : ContentPage
	{
		public SecurityPrepPage ()
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
    }
}