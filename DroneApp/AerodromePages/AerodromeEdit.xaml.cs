using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DroneApp.DataBase;
using SQLiteNetExtensionsAsync.Extensions;
namespace DroneApp.AerodromePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AerodromeEdit : ContentPage
    {
        private int IDAppt;
        private Aerodromes AerodromeToDelete;
        public AerodromeEdit(Aerodromes AerodromeDelete, int ID)
        {
            InitializeComponent();
            AerodromeToDelete = AerodromeDelete;
            IDAppt = ID;
        }
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var Aero = (Aerodromes)BindingContext;
            var Appt = await App.Database.GetItemAsync(IDAppt);
            var ApptEdit = await App.Database.database.GetWithChildrenAsync<Appointment>(Appt.ID);
            ApptEdit.Given_Aerodromes.Remove(AerodromeToDelete);
            ApptEdit.Given_Aerodromes.Add(Aero);
            await App.Database.database.UpdateAllAsync(ApptEdit.Given_Aerodromes);
            await App.Database.database.UpdateWithChildrenAsync(ApptEdit);
            await Navigation.PopAsync();
        }
    }
}