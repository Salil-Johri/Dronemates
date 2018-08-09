using DroneApp.TKMap.ViewModels;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.TKMap.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HtmlInstructionsPage : ContentPage
    {
        public HtmlInstructionsPage(TKRoute route)
        {
            InitializeComponent();

            BindingContext = new HtmlInstructionsViewModel(route);
        }
    }
}