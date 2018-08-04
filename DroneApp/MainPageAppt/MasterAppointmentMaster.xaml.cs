using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.MainPageAppt
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterAppointmentMaster : ContentPage
    {
        public ListView ListView;

        public MasterAppointmentMaster()
        {
            InitializeComponent();

            BindingContext = new MasterAppointmentMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MasterAppointmentMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterAppointmentMenuItem> MenuItems { get; set; }
            
            public MasterAppointmentMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterAppointmentMenuItem>(new[]
                {
                    new MasterAppointmentMenuItem { Id = 0, Title = "Appointments" },
                    new MasterAppointmentMenuItem { Id = 1, Title = "Settings" },
                    new MasterAppointmentMenuItem { Id = 2, Title = "Emergency Plan" }
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}