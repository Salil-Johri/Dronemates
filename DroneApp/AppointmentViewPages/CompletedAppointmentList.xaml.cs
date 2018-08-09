using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;

namespace DroneApp.AppointmentViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletedAppointmentsList : ContentPage
    {
        public CompletedAppointmentsList()
        {
            InitializeComponent();
        }
        async void OnCompletedListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new CompletedSelected
                {
                    BindingContext = e.SelectedItem as Appointment
                });
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ((App)App.Current).ResumeAtTodoId = -1;
            CompletedListView.ItemsSource = await App.Database.GetItemsDoneAsync();
        }
      
    }
}