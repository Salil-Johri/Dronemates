using DroneApp.DataBase;
using DroneApp.TKMap.CustomPins;
using DroneApp.TKMap.ViewModels;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DroneApp.TKMap.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SamplePage : ContentPage
	{
        private Appointment appt; 
        private Position location; 
        private TKCustomMap mapView = new TKCustomMap(); 

        public SamplePage(double longitude, double latitude, Appointment appointment)
        {
            InitializeComponent();
            appt = appointment;
            location = new Position(latitude, longitude);
            mapView = new TKCustomMap(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(4)));
            BindingContext = new SampleViewModel(longitude, latitude, appt);
            CreateView();            
        }

        void CreateView()
        {
            var autoComplete = new PlacesAutoComplete { ApiToUse = PlacesAutoComplete.PlacesApi.Native };
            
            autoComplete.SetBinding(PlacesAutoComplete.PlaceSelectedCommandProperty, "PlaceSelectedCommand");
            
            mapView.SetBinding(TKCustomMap.IsClusteringEnabledProperty, "IsClusteringEnabled");
            mapView.SetBinding(TKCustomMap.GetClusteredPinProperty, "GetClusteredPin");
            mapView.SetBinding(TKCustomMap.PinsProperty, "Pins");
            mapView.SetBinding(TKCustomMap.MapClickedCommandProperty, "MapClickedCommand");
            mapView.SetBinding(TKCustomMap.MapLongPressCommandProperty, "MapLongPressCommand");            
            mapView.MapType = MapType.Hybrid; 
            mapView.SetBinding(TKCustomMap.PinSelectedCommandProperty, "PinSelectedCommand");
            mapView.SetBinding(TKCustomMap.SelectedPinProperty, "SelectedPin");
            mapView.SetBinding(TKCustomMap.RoutesProperty, "Routes");
            mapView.SetBinding(TKCustomMap.PinDragEndCommandProperty, "DragEndCommand");
            mapView.SetBinding(TKCustomMap.CirclesProperty, "Circles");
            mapView.SetBinding(TKCustomMap.CalloutClickedCommandProperty, "CalloutClickedCommand");
            mapView.SetBinding(TKCustomMap.PolylinesProperty, "Lines");
            mapView.SetBinding(TKCustomMap.PolygonsProperty, "Polygons");
            mapView.SetBinding(TKCustomMap.MapRegionProperty, "MapRegion");
            mapView.SetBinding(TKCustomMap.RouteClickedCommandProperty, "RouteClickedCommand");
            mapView.SetBinding(TKCustomMap.RouteCalculationFinishedCommandProperty, "RouteCalculationFinishedCommand");
            mapView.SetBinding(TKCustomMap.TilesUrlOptionsProperty, "TilesUrlOptions");
            mapView.SetBinding(TKCustomMap.MapFunctionsProperty, "MapFunctions");
            mapView.IsRegionChangeAnimated = true;
            mapView.IsShowingUser = true;
            autoComplete.SetBinding(PlacesAutoComplete.BoundsProperty, "MapRegion");

            Content = mapView;
            //_baseLayout.Children.Add(
            //    mapView,
            //    Constraint.RelativeToView(autoComplete, (r, v) => v.X),
            //    Constraint.RelativeToView(autoComplete, (r, v) => autoComplete.HeightOfSearchBar),
            //    heightConstraint: Constraint.RelativeToParent((r) => r.Height - autoComplete.HeightOfSearchBar),
            //    widthConstraint: Constraint.RelativeToView(autoComplete, (r, v) => v.Width));

            //_baseLayout.Children.Add(
            //    autoComplete,
            //    Constraint.Constant(0),
            //    Constraint.Constant(0));
        }
    }
}