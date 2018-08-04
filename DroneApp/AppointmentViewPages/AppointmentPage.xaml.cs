using DroneApp.DataBase;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using DroneApp.FlightPages;
using DroneApp.AerodromePages;
namespace DroneApp.AppointmentViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentPage : ContentPage
    {
        public AppointmentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            startDateJob.MinimumDate = DateTime.Now;
            endDateJob.MinimumDate = DateTime.Now;
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            var todoItem = (Appointment)BindingContext;
            todoItem.Done = 0;
            await App.Database.SaveItemAsync(todoItem);
            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var todoItem = (Appointment)BindingContext;
            await App.Database.DeleteItemAsync(todoItem);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e) => await Navigation.PopAsync();

        async void Current_Location()
        {
            Curr_.IsEnabled = false;
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1));
            ((Appointment)BindingContext).Latitude = position.Latitude;
            ((Appointment)BindingContext).Longitude = position.Longitude;

            var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
            if (answer)
                await Navigation.PushAsync(page: new MapsPage(position.Latitude, position.Longitude));
            Curr_.IsEnabled = true;
        }

        async void OnMorning (object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MorningPrepPage()
            {
                BindingContext = ((Appointment)BindingContext)
            });
        }

        async void OnSite (object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SitePrepPage()
            {
                BindingContext = ((Appointment)BindingContext)

            });
        }

        async void OnSecurity (object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SecurityPrepPage()
            {
                BindingContext = ((Appointment)BindingContext)
            });
        }

        async void OnNormal (object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NormalPrepPage()
            {
                BindingContext = (Appointment)BindingContext
            });
        }

        async void Set_Location()
        {
            Set_.IsEnabled = false;
            var locator = new Geocoder();
            IEnumerable<Position> pos = await locator.GetPositionsForAddressAsync(SetLocation.Text);

            if (pos != null)
            {
                ((Appointment)BindingContext).Longitude = pos.First().Longitude;
                ((Appointment)BindingContext).Latitude = pos.First().Latitude;
                var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
                if (answer)
                    await Navigation.PushAsync(page: new MapsPage(pos.First().Latitude, pos.First().Longitude));
            }
            else
                await DisplayAlert("Error", SetLocation.Text + "could not be found", "Ok");

            Set_.IsEnabled = true;
        }
    }
}