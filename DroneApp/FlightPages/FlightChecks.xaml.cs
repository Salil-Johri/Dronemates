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

        /* OnAppearing: Sets min and max date of NotamDatePicker
         * Pre: None
         * Post: Sets date of NotamDatePicker to the minimum date, if there's no previously saved date
         */
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NotamDatePickerStart.MinimumDate = DateTime.Now;
            NotamDatePickerEnd.MinimumDate = DateTime.Now;
        }
        /* OnViewMap: Navigates to TKMap
         * Pre: None
         * Post: Displays TKMap page, with set location
         */
        private async void OnViewMap(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Would you like to view location on map?", null, "Yes", "No");
            if (answer)
            {
                var appt = await App.Database.database.GetWithChildrenAsync<Appointment>(((Appointment)BindingContext).ID);
                await Navigation.PushAsync(new SamplePage(appt.Longitude, appt.Latitude, appt));
            }
        }
        //Navigates to AerodromePage after button clicked 
        private async void OnAerodromeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AerodromePage() { BindingContext = (Appointment)BindingContext });
        }

        /* OnDisappearing: Saves all data when navigating away from the tabbed page 
         * Pre; None
         * Post: Saves appointment to appointment database
             */
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
        }
        /* UnitConvertAsync: Clicked event for maximum altitude input in site prep: converts meters to feet and vice versa
         * Pre: None
         * Post: Converts units for maximum altitude or radius, and alters the text in the entry field  
         */
        private async void UnitConvertAsync(object sender, EventArgs e)
        {
            await App.Database.SaveItemAsync((Appointment)BindingContext);
            var answer = await DisplayAlert("Are you sure you want to convert this?", null, "Yes", "No");
            if (answer)
            {
                //if want to convert feet to meters
                if ((sender as Button).Text == "To Meter")
                {
                    double feet = ((Appointment)BindingContext).MaxAlt;
                    double meter = feet * 0.31;
                    ((Appointment)BindingContext).MaxAlt = meter;
                    MaxAltEntry.Text = meter.ToString();
                    AltNotam.Text = feet.ToString();
                }
                //convert meters to feet
                else
                {
                    double meter = ((Appointment)BindingContext).MaxAlt;
                    double feet = meter / 0.31;
                    ((Appointment)BindingContext).MaxAlt = feet;
                    MaxAltEntry.Text = feet.ToString();
                    AltNotam.Text = feet.ToString();
                }
            }
        }
        /* OnDroneTypeClicked: DroneType button clicked event function
         * Pre: None
         * Post: Autofills entry fields from drone type info 
         */
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
        /* OnCallClickedAsync: Call button clicked event handler 
         * Pre: None
         * Post: Calls set number based on which button is clicked
         */
        private async void OnCallClickedAsync(object sender, EventArgs e)
        {
            string CallNumber = null;
            //If call contact button clicked - assign Call number to the Contact number
            if ((sender as Button).Text == "Call Contact")
            {
                CallNumber = ((Appointment)BindingContext).ContactNum;
            }
            //If call notam button clicked - assign Call number to the notam number
            else if((sender as Button).Text == "Call NOTAM")
            {
                CallNumber = ((Appointment)BindingContext).NotamPhone;
            }
            //If call acc button clicked = assign callnumber to to the ACC number
            else if((sender as Button).Text == "Call ACC")
            {
                CallNumber = ((Appointment)BindingContext).ACCNumber;
            }

            if (CallNumber != null)
            {
                //confirmation aler to ensure that user does wanty to make call
                if (await DisplayAlert(
                       "Dial a Number",
                       "Would you like to call " + CallNumber + "?",
                       "Yes",
                       "No"))
                {
                    //Call IDialer interface 
                    var dialer = DependencyService.Get<IDialer>();
                    if (dialer != null)
                    {
                        //call Call number
                        dialer.Dial(CallNumber);
                    }                        
                }
            }
            else
            {
                //if Call Number is null, display warning
                await DisplayAlert("Error", "There's no number provided", "Ok");
            }
        }
       
        private void OnWeatherClicked(object sender, EventArgs e)
        {
            //Open weather page  in default internet applicaiton 
            Device.OpenUri(new Uri(uriString: "https://flightplanning.navcanada.ca/cgi-bin/CreePage.pl?Langue=anglais&NoSession=NS_Inconnu&Page=Fore-obs%2Fmetar-taf-map&TypeDoc=html"));
        }
        /* ACCMangagerSelected: Picker index changed event handler
         * Pre: None
         * Post: Set the ACC manager phone number according to seleected index
         */
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
            else if (sitePrepQuestion_7.SelectedIndex == 6)
            {
                ((Appointment)BindingContext).ACCNumber = "12049838338";
            }
        }
        /* OnSelectedIndexChanged: Selected index xhanged event handler for flight class picker
         * Pre: None
         * Post: If class G is selected the section 8 title is altered to notify user that the fields for that section are not required 
         */
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //if class g is selected, change section 8 title to inform user
            //that section 8 is not required for class g airspace 
            if ((string)(sitePrepQuestion_2.SelectedItem) == "Class G")
            {
                Section8_Title.TextColor = Color.Red;
                Section8_Title.Text = "8. NOTAM File (NOTE: CLASS G IS SELECTED. NOTAM MAY NOT NEED TO BE FILED.)";
            }
            //change back to default if the index is changed from section 
            else
            {
                Section8_Title.TextColor = Color.Black;
                Section8_Title.Text = "8. NOTAM File";
            }
        }
        /*OnAreaClicked: Unit conversion for maximum altitude entry 
         * Pre: None
         * Post: Converts units for maximum altitude in NOTAM section, and alters the text in the entry field 
         */
        private void OnAreaClicked(object sender, EventArgs e)
        {
            //converts to meters from feet
            if ((sender as Button).Text == "To Meters")
            {
                double feet = ((Appointment)BindingContext).MaxAlt;
                double meter = feet * 0.31;
                ((Appointment)BindingContext).MaxAlt = meter;
                //changes the entry field to contain the meter value
                AltNotam.Text = meter.ToString();
            }
            //converts to feet from meters
            else
            {
                double meter = ((Appointment)BindingContext).MaxAlt;
                double feet = meter / 0.31;
                ((Appointment)BindingContext).MaxAlt = feet;
                //changes the entry field to contain the feet value 
                AltNotam.Text = feet.ToString();
            }
        }
        /*OnAreaClicked: Unit conversion for maximum altitude entry
        * Pre: None
        * Post: Converts units for radius in NOTAM section, and alters the text in the entry field 
        */
        private void OnRadiusAreaClicked(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "To Meters")
            {
                double nm = ((Appointment)BindingContext).RadiusNM;
                double meter = nm * 1852;
                ((Appointment)BindingContext).RadiusNM = meter;
                //changes the entry field to contain the meter value
                RadiusNotam.Text = meter.ToString();
                radiusQuestion3.Text = meter.ToString();
            }
            else
            {
                double meter = ((Appointment)BindingContext).RadiusNM;
                double nm = meter / 1852;
                ((Appointment)BindingContext).RadiusNM = nm;
                //changes the entry field to contain the meter value
                RadiusNotam.Text = nm.ToString();
                radiusQuestion3.Text = nm.ToString();
            }
        }
    }
}