using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
using DroneApp.AerodromePages;
namespace DroneApp.FlightPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SitePrepPage : ContentPage
	{
		public SitePrepPage ()
		{
			InitializeComponent ();
		}

        async void OnSaveClicked(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
            await Navigation.PopAsync();
        }

        async void OnAerodromeButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text == "Add Nearby Aerodromes")
            {
                await Navigation.PushAsync(new AerodromeAdd()
                {
                    BindingContext = ((Appointment)BindingContext)
                });
            }
            else
            {
                await Navigation.PushAsync(new AerodromePage()
                {
                    BindingContext = ((Appointment)BindingContext)

                });
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)(sitePrepQuestion_2.SelectedItem) == "Class G")
            {
                Site_PrepQ8.IsVisible = false;
            }
            else
            {
                Site_PrepQ8.IsVisible = true;
            }
        }
    }
}