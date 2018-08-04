using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace DroneApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapsPage : ContentPage
    {
        public MapsPage(double lat, double longit)
        {
            InitializeComponent();
            var map = new Map(
            MapSpan.FromCenterAndRadius(
                  new Position(lat, longit), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MapType = MapType.Satellite
            };
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;

            var position = new Position(lat, longit); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Generic,
                Position = position,
                Label = "custom pin",
                Address = "custom detail info"
            };
            map.Pins.Add(pin);
        }
    }
}