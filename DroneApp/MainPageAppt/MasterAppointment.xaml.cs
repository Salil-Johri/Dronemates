using DroneApp.AppointmentViewPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.MainPageAppt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterAppointment : MasterDetailPage
    {
        public MasterAppointment()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterAppointmentMenuItem;
            if (item == null)
                return;

            if (item.Title == "Settings")
            {
                Detail = new NavigationPage(new Settings());
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                IsPresented = false;
                MasterPage.ListView.SelectedItem = null;
            }
            else if (item.Title == "Appointments")
            {
                Detail = new NavigationPage(new MasterAppointmentDetail());
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                IsPresented = false;
                MasterPage.ListView.SelectedItem = null;
            }
            else if (item.Title == "Emergency Plan")
            {
                Detail = new NavigationPage(new EmergencyPage());
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                IsPresented = false;
                MasterPage.ListView.SelectedItem = null;
            }
        }
    }
}