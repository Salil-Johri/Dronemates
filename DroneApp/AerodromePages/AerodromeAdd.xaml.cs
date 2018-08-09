using System;
using SQLiteNetExtensionsAsync.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
namespace DroneApp.AerodromePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AerodromeAdd : ContentPage
    {
        public AerodromeAdd()
        {
            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs e)
        {
            var aerodrome = new Aerodromes
            {
                AeroCode = aeroCode.Text,
                AeroDirection = aeroDirection.Text,
                AeroDistance = aeroDistance.Text,
                AeroName = aeroName.Text,
                AeroPhone = aeroPhone.Text,
                AeroATF = aeroATF.Text
            };
            var apptUpdate = await App.Database.database.GetWithChildrenAsync<Appointment>(((Appointment)BindingContext).ID);
            apptUpdate.Given_Aerodromes.Add(aerodrome);
            await App.Database.database.InsertAllAsync(apptUpdate.Given_Aerodromes);
            await App.Database.database.UpdateWithChildrenAsync(apptUpdate);
        }
        /* OnNRGC: Event handler that navigates to a UAV site selection tool on an external browser on the user's device 
         * Pre: none 
         * Post: The given website is navigated to on chrome
         */
        private async void OnNRGC(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("WARNING", "Are you sure you want to navigate out of app?", "Yes", "No");
            if (answer)
            {
                Device.OpenUri(new Uri("https://www.nrc-cnrc.gc.ca/eng/solutions/collaborative/civuas/uav_site_selection_tool.html"));
            }

        }

        /* OnNRGC: Event handler that navigates to a Aerodrome locator website on an external browser on the user's device 
         * Pre: none 
         * Post: The given website is navigated to on chrome
         */
        private async void OnSkyVector(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("WARNING", "Are you sure you want to navigate out of app?", "Yes", "No");
            if (answer)
            {
                Device.OpenUri(new Uri("https://skyvector.com/airports"));
            }
        }
    }
}