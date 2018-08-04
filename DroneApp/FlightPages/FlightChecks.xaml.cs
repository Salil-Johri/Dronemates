using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
using DroneApp.AerodromePages;
namespace DroneApp.FlightPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightChecks : TabbedPage
    {
        public FlightChecks() => InitializeComponent();
        async void OnSaveClicked(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
        }
        private void OnViewMap(object sender, EventArgs e)
        {
            //var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
            //if (answer)
            //    await Navigation.PushAsync(page: new MapPage(((Appointment)BindingContext).Latitude, ((Appointment)BindingContext).Longitude, ((Appointment)BindingContext).Address));
        }
        async void OnAerodromeButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new AerodromePage() { BindingContext = ((Appointment)BindingContext)});
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)(sitePrepQuestion_2.SelectedItem) == "Class G")
            {
                Site_PrepQ8.IsVisible = false;
            } else
            {
                Site_PrepQ8.IsVisible = true;
            }
        }

    }
}