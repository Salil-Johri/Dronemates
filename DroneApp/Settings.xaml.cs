using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : TabbedPage
    {
        public Settings ()
        {
            InitializeComponent();
        }
        void ChangeFontSize(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            label.Text = String.Format("The Slider value is {0}", value);
            label.FontSize = slider.Value; 
        }
    }
}