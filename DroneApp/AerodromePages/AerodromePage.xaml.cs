using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
using SQLiteNetExtensionsAsync.Extensions;
using DroneApp.AerodromePages;
using DroneApp.PhoneDialer;

namespace DroneApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AerodromePage : ContentPage
    {
        public AerodromePage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ((App)App.Current).ResumeAtTodoId = -1;
            //Finds appointment saved in DB
            var curr_appt = await App.Database.GetItemAsync(((Appointment)BindingContext).ID);
            //Gets children of appointment
            var appt = await App.Database.database.GetWithChildrenAsync<Appointment>(curr_appt.ID);
            AerodromeList.ItemsSource = appt.Given_Aerodromes;
        }
        //Tool bar add clicked 
        async void OnClicked(object sender, EventArgs e) => await Navigation.PushAsync(new AerodromeAdd() { BindingContext = ((Appointment)BindingContext) });
        //List item selected
        async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new AerodromeEdit(e.SelectedItem as Aerodromes, ((Appointment)BindingContext).ID) { BindingContext = e.SelectedItem as Aerodromes });
        }
        async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var answer = await DisplayAlert("Are you sure?", null, "Yes", "No");

            // If user answers yes to the alert
            if (answer)
            {
                // Delete aerodrome from database
                var aerodrome = (Aerodromes)mi.CommandParameter;

                // Find appointment that aerodrome is related to, remove aerodrome from given list
                var curr_appt = await App.Database.GetItemAsync(((Appointment)BindingContext).ID);
                var appt = await App.Database.database.GetWithChildrenAsync<Appointment>(curr_appt.ID);
                appt.Given_Aerodromes.Remove(aerodrome);

                // Delete aerodrome from the database
                await App.Database.DeleteAerodromeAsync(aerodrome);

                // Update appointment object in database, refresh list view. 
                await App.Database.database.UpdateWithChildrenAsync(appt);
                AerodromeList.ItemsSource = appt.Given_Aerodromes;
            }
        }
        private async void OnCall(object sender, EventArgs e)
        {
            string CallNumber = null;
            var mi = sender as MenuItem;

            var aero = mi.CommandParameter as Aerodromes;

            // Gets phone number of selected aerodrome
            CallNumber = aero.AeroPhone;

            // If there is a call number
            if (CallNumber != null)
            {
                // Displays alert ensuring user wants to call number

                var answer = await this.DisplayAlert(
                       "Dial a Number",
                       "Would you like to call " + CallNumber + "?",
                       "Yes",
                       "No");

                // If user answers yes, app navigates to dialer.
                if (answer)
                {
                    var dialer = DependencyService.Get<IDialer>();
                    if (dialer != null)
                        dialer.Dial(CallNumber);
                }
            }
            else
            {
                await DisplayAlert("Error", "There's no number provided", "Ok");
            }
        }
    }
}