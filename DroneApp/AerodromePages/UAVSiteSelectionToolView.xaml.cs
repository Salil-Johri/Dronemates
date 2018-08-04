using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.AerodromePages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UAVSiteSelectionToolView : ContentPage
	{
		public UAVSiteSelectionToolView ()
		{
			InitializeComponent ();
		}

        private void OnBack(object sender, EventArgs e)
        {
            if(Browser.CanGoBack)
            {
                Browser.GoBack();
            } else
            {
                Navigation.PopAsync();
            }
        }

        private void OnForward(object sender, EventArgs e)
        {
            if (Browser.CanGoForward)
            {
                Browser.GoForward();
            }
        }
	}
}