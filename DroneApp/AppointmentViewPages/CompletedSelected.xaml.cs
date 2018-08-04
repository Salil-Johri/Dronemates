using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;

namespace DroneApp.AppointmentViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletedSelected : ContentPage
    {
        public CompletedSelected()
        {
            InitializeComponent();
        }
        private async void View_Location(object sender, EventArgs e)
        {
            //View_Button.IsEnabled = false;
            //await Navigation.PushAsync(new MapPage(((Appointment)BindingContext).Latitude, ((Appointment)BindingContext).Longitude, ((Appointment)BindingContext).Address));
            //View_Button.IsEnabled = true;
        }
        private async void NotCompleted(object sender, EventArgs e)
        {
            var Appt = ((Appointment)BindingContext);
            Appt.Done = 0;
            await App.Database.SaveItemAsync(Appt);
            await Navigation.PopAsync();
        }
    }
}