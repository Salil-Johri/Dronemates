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
            await Navigation.PopAsync();
        }


        private void OnNRGC(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.nrc-cnrc.gc.ca/eng/solutions/collaborative/civuas/uav_site_selection_tool.html"));
        }

        private void OnSkyVector(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://skyvector.com/airports"));
        }
    }
}