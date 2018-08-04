using System;
using SQLiteNetExtensionsAsync.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
namespace DroneApp.AerodromePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AerodromePage : ContentPage
    {
        public AerodromePage()
        {
            InitializeComponent();
        }

        /* On Appearing method sets up list view for when user navigates to apps 
         * Pre: none
         * Post: Listview item source is set to list of nearby aerodromes that user has saved currently. 
         */
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ((App)App.Current).ResumeAtTodoId = -1;
            // Gets the current appointment that has been selected, including its children objects (i.e. the given aerodromes)
            var curr_appt = await App.Database.GetItemAsync(((Appointment)BindingContext).ID);
            var appt = await App.Database.database.GetWithChildrenAsync<Appointment>(curr_appt.ID);
            // Item source of listview becomes given appointment's 
            AerodromeListView.ItemsSource = appt.Given_Aerodromes;
        }


        /* On Item Tapped event handler that navigates to a page to edit a given aerodrome list item
          * Pre: none
          * Post: page navigates to edit aerodrome page, where the binding context is the selected aerodrome
          */
        async void OnItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            
            if (e.SelectedItem != null)
            {
                Aerodromes AerodromeDelete = e.SelectedItem as Aerodromes;
                await Navigation.PushAsync(new AerodromeEdit(AerodromeDelete, ((Appointment)BindingContext).ID)
                {
                    BindingContext = e.SelectedItem as Aerodromes
                });
            }
        }

        /* On Item added event handler that navigates to a page to add a new aerodrome list item
          * Pre: none
          * Post: page navigates to add aerodrome page, where the binding context is the appointment, so that the aerodrome can be added to the list within the appointment
          *       object
          */
        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AerodromeAdd
            {
                BindingContext = ((Appointment)BindingContext)
            });
        }


        /* On Delete event handler that Deletes a selected aerodrome
          * Pre: none
          * Post: Aerodrome is deleted from the database, as well as the list it is related to
          */
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
                AerodromeListView.ItemsSource = appt.Given_Aerodromes;
            }
        }
    }
}
