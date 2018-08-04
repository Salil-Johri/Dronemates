using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
namespace DroneApp.FlightPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MorningPrepPage : ContentPage
	{
		public MorningPrepPage ()
		{
			InitializeComponent ();
		}

        async void OnSaveClicked(object sender, EventArgs e)
        {
            var apptitem = (Appointment)BindingContext;
            await App.Database.SaveItemAsync(apptitem);
            await Navigation.PopAsync();
        }
    }
}