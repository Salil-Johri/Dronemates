using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
using DroneApp.TKMap.Pages;

namespace DroneApp.AppointmentViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletedSelected : ContentPage
    {
        public CompletedSelected()
        {
            InitializeComponent();
        }
        async void View_Location(object sender, EventArgs e)
        {
            View_Button.IsEnabled = false;

            var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
            if (answer)
            {
                await Navigation.PushAsync(new SamplePage(((Appointment)BindingContext).Longitude, ((Appointment)BindingContext).Latitude, (Appointment)BindingContext));
            }
            View_Button.IsEnabled = true;
        }
        async void NotCompleted(object sender, EventArgs e)
        {
            var Appt = ((Appointment)BindingContext);
            Appt.Done = 0;
            await App.Database.SaveItemAsync(Appt);
            await Navigation.PopAsync();
        }
    }
}