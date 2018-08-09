using DroneApp.DataBase;
using DroneApp.PhoneDialer;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.FlightPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NormalPrepPage : ContentPage
	{
		public NormalPrepPage ()
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
        private async void OnManagerCall(object sender, EventArgs e)
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
                    normalProcedureQuestion_1.SelectedIndex = 0;
                    ((Appointment)BindingContext).AccCalled = "Yes";
                    var dialer = DependencyService.Get<IDialer>();
                    if (dialer != null)
                        dialer.Dial(CallNumber);
                }
            }
            else
                await DisplayAlert("Error", "There's no number provided", "Ok");
        }
    }
}