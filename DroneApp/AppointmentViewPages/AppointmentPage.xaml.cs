using DroneApp.DataBase;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using DroneApp.FlightPages;
using DroneApp.TKMap.Pages;

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
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
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

            double lat_ = position.Latitude;
            double long_ = position.Longitude;

            Geocoder pos = new Geocoder();
            Xamarin.Forms.Maps.Position pos_ = new Xamarin.Forms.Maps.Position(lat_, long_);
            var address = await pos.GetAddressesForPositionAsync(pos_); 

            
            ((Appointment)BindingContext).Latitude = position.Latitude;
            ((Appointment)BindingContext).Longitude = position.Longitude;
            ((Appointment)BindingContext).Address = address.First().ToString(); 

            var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
            if (answer)
            {
                await Navigation.PushAsync(new SamplePage(((Appointment)BindingContext).Longitude, ((Appointment)BindingContext).Latitude, (Appointment)BindingContext));
            }
            Curr_.IsEnabled = true;
        }

        async void Set_Location()
        {
            Set_.IsEnabled = false;
            var locator = new Geocoder(); 
            IEnumerable<Xamarin.Forms.Maps.Position> pos = await locator.GetPositionsForAddressAsync(SetLocation.Text);
            ((Appointment)BindingContext).Address = SetLocation.Text; 
            if (pos != null)
            {
                ((Appointment)BindingContext).Longitude = pos.First().Longitude;
                ((Appointment)BindingContext).Latitude = pos.First().Latitude;
                var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
                if (answer)
                {
                    await Navigation.PushAsync(new SamplePage(((Appointment)BindingContext).Longitude, ((Appointment)BindingContext).Latitude, (Appointment)BindingContext));
                }
            }
            else
                await DisplayAlert("Error", SetLocation.Text + "could not be found", "Ok");

            Set_.IsEnabled = true;
        }
        async void OnFlightPages(object sender, EventArgs e) => await Navigation.PushAsync(new FlightChecks() { BindingContext = (Appointment)BindingContext });
        async void OnFlightPagesClicked(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Site")
                await Navigation.PushAsync(new SitePrepPage() { BindingContext = (Appointment)BindingContext });

            else if ((sender as Button).Text == "Morning")
                await Navigation.PushAsync(new MorningPrepPage() { BindingContext = (Appointment)BindingContext });

            else if ((sender as Button).Text == "Normal")
                await Navigation.PushAsync(new NormalPrepPage() { BindingContext = (Appointment)BindingContext });

            else
                await Navigation.PushAsync(new SecurityPrepPage() { BindingContext = (Appointment)BindingContext }); 
        }
    }
}