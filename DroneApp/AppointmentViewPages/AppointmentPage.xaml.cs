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
using System.Reflection;
using SQLiteNetExtensionsAsync.Extensions;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;

namespace DroneApp.AppointmentViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentPage : ContentPage
    {
        public AppointmentPage()
        {
            InitializeComponent();
        }
        //Sates Date picker defaul to current date 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            startDateJob.MinimumDate = DateTime.Now;
            endDateJob.MinimumDate = DateTime.Now;
        }
        //Saves information when leaving the page 
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
        }
        //Save Appointment 
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var todoItem = (Appointment)BindingContext;
            todoItem.Done = 0;
            await App.Database.SaveItemAsync(todoItem);
            await Navigation.PopAsync();
        }
        //Delete Appointment
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var todoItem = (Appointment)BindingContext;
            await App.Database.DeleteItemAsync(todoItem);
            await Navigation.PopAsync();
        }
        //Cancel creating appointment
        private async void OnCancelClicked(object sender, EventArgs e) => await Navigation.PopAsync();

        private void OnLocationOption(object sender, EventArgs e)
        {
            if((string)(SelectedLocationOptions.SelectedItem) == "Use Current Location")
            {
                Current_Location(); 
                Curr_.IsVisible = true;
                LocationLayout.IsVisible = false;
            }
            else
            {
                Curr_.IsVisible = false;
                LocationLayout.IsVisible = true; 
            }
        }

        //Sets location of appointment to current location, by assigning the longitude and latitude. Also navigates to TKMap to view location and map items 
        private async void Current_Location()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1));

            double lat_ = position.Latitude;
            double long_ = position.Longitude;

            Geocoder pos = new Geocoder();
            Position pos_ = new Position(lat_, long_);
            var address = await pos.GetAddressesForPositionAsync(pos_); 

            
            ((Appointment)BindingContext).Latitude = position.Latitude;
            ((Appointment)BindingContext).Longitude = position.Longitude;
            ((Appointment)BindingContext).Address = address.First().ToString();
            DegMinSecConversion();
            var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
            if (answer)
            {
                await Navigation.PushAsync(new SamplePage(((Appointment)BindingContext).Longitude, ((Appointment)BindingContext).Latitude, (Appointment)BindingContext));
            }
        }
        //Sets location based on string entry 
        private async void Set_Location()
        {
            Set_.IsEnabled = false;
            var locator = new Geocoder(); 
            IEnumerable<Position> pos = await locator.GetPositionsForAddressAsync(SetLocation.Text);
            ((Appointment)BindingContext).Address = SetLocation.Text; 
            if (pos != null)
            {
                ((Appointment)BindingContext).Longitude = pos.First().Longitude;
                ((Appointment)BindingContext).Latitude = pos.First().Latitude;
                DegMinSecConversion();
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

        private async void OnFlightPages(object sender, EventArgs e) => await Navigation.PushAsync(new FlightChecks() { BindingContext = (Appointment)BindingContext });

        private async void WriteTXT()
        {
            Appointment appt = (Appointment)BindingContext;
            string content = "";
            foreach (PropertyInfo prop in typeof(Appointment).GetProperties())
            {
                if (prop.GetValue(appt, null) == null)
                {
                    content += prop.Name + " = No Value Saved\n";
                }
                else if (prop.Name == "Given_Aerodromes")
                {
                    continue; 
                }
                else
                {
                    content += prop.Name + " = " + prop.GetValue(appt, null) + "\n";
                }
            }
            content += "\n" + "<-------------------------------------->" + "\n" + "Aerodromes List:";

            var appt_ = await App.Database.database.GetWithChildrenAsync<Appointment>(((Appointment)BindingContext).ID); 
            List<Aerodromes> Aero = appt_.Given_Aerodromes;
            if (Aero != null)
            {
                int NumAero = Aero.Count;
                for (int i = 0; i < NumAero; i++)
                {
                    content += "\n" + "AERODROME " + (i + 1) + "\n";
                    Aerodromes CurrAerodrome = Aero.ElementAt(i);
                    foreach (PropertyInfo property in typeof(Aerodromes).GetProperties())
                    {
                        if (property.GetValue(CurrAerodrome, null) == null)
                        {
                            content += property.Name + " = No Value Saved\n";
                        }
                        else
                        {
                            content += property.Name + " = " + property.GetValue(CurrAerodrome, null) + "\n";
                        }
                    }
                }
            }
            else
            {
                content += "No Aerodromes for this project\n";
            }
            content += "<-------------------------------------->\n";

            Device.OpenUri(new Uri("mailto:evanlavine@dronemates.ca?subject=" + ((Appointment)BindingContext).Name + "&body=" + content));
        }
        /* DegMinSecConversion: Converts Latitude and Longitude coordinates to Degrees, Minutes, Seconds and Direction.
         * Pre: Location must be set prior to function being called
         * Post: Latitude and Longitude of the appointment location is converted, and fields of database are updated.
         */
        private void DegMinSecConversion()
        {
            double lat = ((Appointment)BindingContext).Latitude;
            double longit = ((Appointment)BindingContext).Longitude;

            // Converts Latitude. Formula can be found online
            int degLat = (int)(Math.Abs(lat));
            int minLat = (int)((Math.Abs(lat) - Math.Abs(degLat)) * 60);
            double secLat = ((double)(Math.Abs(lat) - Math.Abs(degLat)) - (double)(minLat / 60.0)) * 3600;
            double absLong = Math.Abs(longit);

            // Converts Longitude
            int degLong = (int)(Math.Abs(longit));
            int minLong = (int)((Math.Abs(longit) - Math.Abs(degLong)) * 60);
            double secLong = ((double)(Math.Abs(longit) - Math.Abs(degLong)) - (double)(minLong / 60.0)) * 3600;

            // Sets direction of Latitude based on sign of number
            if (lat < 0)
            {
                ((Appointment)BindingContext).directionLat = "South";
            }
            else if (lat > 0)
            {
                ((Appointment)BindingContext).directionLat = "North";
            }


            // Sets direction of Longutde based on sign of number
            if (longit < 0)
            {
                ((Appointment)BindingContext).directionLong = "West";
            }
            else if (longit > 0)
            {
                ((Appointment)BindingContext).directionLong = "East";
            }

            // Updates fields in object with converted values
            ((Appointment)BindingContext).degreesLat = degLat;
            ((Appointment)BindingContext).degreesLong = degLong;
            ((Appointment)BindingContext).minutesLat = minLat;
            ((Appointment)BindingContext).minutesLong = minLong;
            ((Appointment)BindingContext).secondsLat = secLat;
            ((Appointment)BindingContext).secondsLong = secLong;
        }
        /* OnFileClicked: Event handler that navigates to the File Explorer on the phone to find 
         *                documents that need to be accessed. 
         * Pre: None
         * Post: App navigates to file picker. 
         */
        //private async void OnFileClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var answer = await DisplayAlert("WARNING", "Are you sure you want to navigate out of app?", "Yes", "No");

        //        if (answer)
        //        {
        //            FileData filedata = await CrossFilePicker.Current.PickFile();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.ToString());
        //    }
        //}
    }
}