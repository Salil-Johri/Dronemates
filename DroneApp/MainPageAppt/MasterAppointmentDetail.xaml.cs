using DroneApp.AppointmentViewPages;
using DroneApp.DataBase;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.MainPageAppt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterAppointmentDetail : ContentPage
    {
        public MasterAppointmentDetail()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ((App)App.Current).ResumeAtTodoId = -1;
            listView.ItemsSource = await App.Database.GetItemsNotDoneAsync();
        }
        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppointmentPage() { BindingContext = new Appointment()});
        }
        //Toolbar button clicked 
        async void OnDoneClicked(object sender, EventArgs e) => await Navigation.PushAsync(new CompletedAppointmentsList());

        async void OnItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                 await Navigation.PushAsync(new AppointmentPage
                 {
                     BindingContext = e.SelectedItem as Appointment
                 }); 
            }
        }
        //Context action button 
        async void OnDone(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var answer = await DisplayAlert("Are you sure?", null, "Yes", "No");

            if (answer)
            {
                ((Appointment)mi.CommandParameter).Done = 1;
                await App.Database.SaveItemAsync((Appointment)mi.CommandParameter);
                listView.ItemsSource = await App.Database.GetItemsNotDoneAsync();
            }
        }
        async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var answer = await DisplayAlert("Are you sure?", null, "Yes", "No");

            if (answer)
            {
                await App.Database.DeleteItemAsync((Appointment)mi.CommandParameter);
                listView.ItemsSource = await App.Database.GetItemsNotDoneAsync(); 
            }
        }
        async void OnInfo(object sender, SelectedItemChangedEventArgs e)
        {
            var mi = ((MenuItem)sender);
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new FlightPages.FlightChecks() { BindingContext = mi.CommandParameter as Appointment });
            }
        }
    }
}