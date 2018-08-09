using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TK.CustomMap;
using TK.CustomMap.Api;
using TK.CustomMap.Api.Google;
using TK.CustomMap.Api.OSM;
using TK.CustomMap.Interfaces;
using TK.CustomMap.Overlays;
using DroneApp.TKMap.Pages; 
using Xamarin.Forms;
using DroneApp.TKMap.CustomPins;
using DroneApp.DataBase;
using SQLiteNetExtensionsAsync.Extensions;
using System.Reflection;

namespace DroneApp.TKMap.ViewModels
{
    public class SampleViewModel : INotifyPropertyChanged
    {
        public static double longitude;
        public static double latitude; 
        Appointment appointment;

        TKTileUrlOptions _tileUrlOptions;

        MapSpan _mapRegion = MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromKilometers(0.12192));
        Position _mapCenter;
        TKCustomMapPin _selectedPin;
        bool _isClusteringEnabled;
        ObservableCollection<TKCustomMapPin> _pins;
        ObservableCollection<TKRoute> _routes;
        ObservableCollection<TKCircle> _circles;
        ObservableCollection<TKPolyline> _lines = new ObservableCollection<TKPolyline>();
        ObservableCollection<TKPolygon> _polygons = new ObservableCollection<TKPolygon>(); 
        List<Pins> pinlist;
        List<Circles> circlelist;
        Random _random = new Random(1984);

        public TKTileUrlOptions TilesUrlOptions
        {
            get
            {
                return _tileUrlOptions;
                //return new TKTileUrlOptions(
                //    "http://a.basemaps.cartocdn.com/dark_all/{2}/{0}/{1}.png", 256, 256, 0, 18);
                //return new TKTileUrlOptions(
                //    "http://a.tile.openstreetmap.org/{2}/{0}/{1}.png", 256, 256, 0, 18);
            }
            set
            {
                if (_tileUrlOptions != value)
                {
                    _tileUrlOptions = value;
                    OnPropertyChanged("TilesUrlOptions");
                }
            }
        }

        public IRendererFunctions MapFunctions { get; set; }       

        public bool IsClusteringEnabled
        {
            get => _isClusteringEnabled;
            set
            {
                _isClusteringEnabled = value;
                OnPropertyChanged(nameof(IsClusteringEnabled));
            }
        }

        public Command ShowListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (_pins == null || !_pins.Any())
                    {
                        await Application.Current.MainPage.DisplayAlert("Nothing there!", "No pins to show!", "OK");
                        return;
                    }
                    var listPage = new PinListPage(Pins);
                    listPage.PinSelected += async (o, e) =>
                    {
                        SelectedPin = e.Pin;
                        await Application.Current.MainPage.Navigation.PopAsync();
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(listPage);
                });
            }
        }

        /// <summary>
        /// Map region bound to <see cref="TKCustomMap"/>
        /// </summary>
        public MapSpan MapRegion
        {
            get { return _mapRegion; }
            set
            {
                if (_mapRegion != value)
                {
                    _mapRegion = value;
                    OnPropertyChanged("MapRegion");
                }
            }
        }



        /// <summary>
        /// Pins bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public ObservableCollection<TKCustomMapPin> Pins
        {
            get { return _pins; }
            set
            {
                if (_pins != value)
                {
                    _pins = value;
                    OnPropertyChanged("Pins");
                }
            }
        }

        /// <summary>
        /// Routes bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public ObservableCollection<TKRoute> Routes
        {
            get { return _routes; }
            set
            {
                if (_routes != value)
                {
                    _routes = value;
                    OnPropertyChanged("Routes");
                }
            }
        }

        /// <summary>
        /// Circles bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public ObservableCollection<TKCircle> Circles
        {
            get { return _circles; }
            set
            {
                if (_circles != value)
                {
                    _circles = value;
                    OnPropertyChanged("Circles");
                }
            }
        }

        /// <summary>
        /// Lines bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public ObservableCollection<TKPolyline> Lines
        {
            get { return _lines; }
            set
            {
                if (_lines != value)
                {
                    _lines = value;
                    OnPropertyChanged("Lines");
                }
            }
        }

        /// <summary>
        /// Polygons bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public ObservableCollection<TKPolygon> Polygons
        {
            get { return _polygons; }
            set
            {
                if (_polygons != value)
                {
                    _polygons = value;
                    OnPropertyChanged("Polygons");
                }
            }
        }

        /// <summary>
        /// Map center bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Position MapCenter
        {
            get { return _mapCenter; }
            set
            {
                if (_mapCenter != value)
                {
                    _mapCenter = value;
                    OnPropertyChanged("MapCenter");
                }
            }
        }

        /// <summary>
        /// Selected pin bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public TKCustomMapPin SelectedPin
        {
            get { return _selectedPin; }
            set
            {
                if (_selectedPin != value)
                {
                    _selectedPin = value;
                    OnPropertyChanged("SelectedPin");
                }
            }
        }

        /// <summary>
        /// Map Long Press bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Command<Position> MapLongPressCommand
        {
            get
            {
                return new Command<Position>(async position =>
                {
                    var action = await Application.Current.MainPage.DisplayActionSheet(
                        "Long Press",
                        "Cancel",
                        null,
                        "Add Pin",
                        "Add Circle", 
                        "Add Region Pin");

                    if (action == "Add Pin")
                    {
                        var pin = new TKCustomMapPin
                        {
                            Position = position,
                            Title = string.Format("Pin {0}, {1}", position.Latitude, position.Longitude),
                            ShowCallout = true,
                            IsDraggable = true,
                            IsCalloutClickable = true
                        };
                        var pin_ = new Pins
                        {
                            Latitude = position.Latitude,
                            Longitude = position.Longitude, 
                            isPolyline = false
                        };

                        var apptUpdate = await App.Database.database.GetWithChildrenAsync<Appointment>(appointment.ID);
                        apptUpdate.PinList.Add(pin_);
                        pinlist.Add(pin_);
                        await App.Database.database.InsertAllAsync(apptUpdate.PinList);
                        await App.Database.database.UpdateWithChildrenAsync(apptUpdate);

                        _pins.Add(pin);
                    }
                    else if (action == "Add Circle")
                    {
                        var circle = new TKCircle
                        {
                            Center = position,
                            Radius = 30,
                            Color = Color.FromRgba(100, 0, 0, 80)
                        };
                        var circle_ = new Circles
                        {
                            Longitude = position.Longitude,
                            Latitude = position.Latitude
                        };
                        var apptUpdate = await App.Database.database.GetWithChildrenAsync<Appointment>(appointment.ID);
                        apptUpdate.CircleList.Add(circle_);
                        circlelist.Add(circle_);
                        await App.Database.database.InsertAllAsync(apptUpdate.CircleList);
                        await App.Database.database.UpdateWithChildrenAsync(apptUpdate);

                        _circles.Add(circle);
                    }
                    else if (action == "Add Region Pin")
                    {
                        var pin = new TKCustomMapPin
                        {
                            Position = position,
                            Title = string.Format("Pin {0}, {1}", position.Latitude, position.Longitude),
                            ShowCallout = false,
                            IsDraggable = false,
                            IsCalloutClickable = false,
                            DefaultPinColor = Color.DarkBlue
                        };
                        
                        var pin_ = new Pins
                        {
                            Latitude = position.Latitude,
                            Longitude = position.Longitude, 
                            isPolyline = true 
                        }; 

                        pinlist.Add(pin_); 
                        _pins.Add(pin);

                        if(pinlist.Count >= 3)
                        {
                            var list = new List<Position>();
                            double lat_ = 0, long_ = 0; 
                            for(int count = 0; count < pinlist.Count; count++)
                            {
                                if(pinlist.ElementAt(count).isPolyline == true)
                                {
                                    lat_ = pinlist.ElementAt(count).Latitude;
                                    long_ = pinlist.ElementAt(count).Longitude;

                                    list.Add(new Position(lat_, long_)); 
                                }
                            }
                            var poly = new TKPolygon
                            {
                                StrokeColor = Color.DarkBlue,
                                StrokeWidth = 10f, 
                                Color = Color.FromRgba(0,0,0,0),
                                Coordinates = list
                            };
                            if(pinlist.Count > 3)
                            {
                                _polygons.Clear(); 
                            }
                            _polygons.Add(poly);
                        }

                        var apptUpdate = await App.Database.database.GetWithChildrenAsync<Appointment>(appointment.ID);
                        apptUpdate.PinList.Add(pin_);
                        await App.Database.database.InsertAllAsync(apptUpdate.PinList);
                        await App.Database.database.UpdateWithChildrenAsync(apptUpdate);
                    }
                });
            }
        }

        /// <summary>
        /// Map Clicked bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Command<Position> MapClickedCommand
        {
            get
            {
                return new Command<Position>((positon) =>
                {
                    SelectedPin = null;

                    // Determine if a point was inside a circle
                    if ((from c in _circles let distanceInMeters = c.Center.DistanceTo(positon) * 1000 where distanceInMeters <= c.Radius select c).Any())
                    {
                        Application.Current.MainPage.DisplayAlert("Circle tap", "Circle was tapped", "OK");
                    }
                });
            }
        }

        /// <summary>
        /// Command when a place got selected
        /// </summary>
        public Command<IPlaceResult> PlaceSelectedCommand
        {
            get
            {
                return new Command<IPlaceResult>(async p =>
                {
                    var gmsResult = p as GmsPlacePrediction;
                    if (gmsResult != null)
                    {
                        var details = await GmsPlace.Instance.GetDetails(gmsResult.PlaceId);
                        MapCenter = new Position(details.Item.Geometry.Location.Latitude, details.Item.Geometry.Location.Longitude);
                        return;
                    }
                    var osmResult = p as OsmNominatimResult;
                    if (osmResult != null)
                    {
                        MapCenter = new Position(osmResult.Latitude, osmResult.Longitude);
                        return;
                    }
                    var prediction = (TKNativeAndroidPlaceResult)p;

                    var details_ = await TKNativePlacesApi.Instance.GetDetails(prediction.PlaceId);

                    MapCenter = details_.Coordinate;

                });
            }
        }

        /// <summary>
        /// Pin Selected bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Command PinSelectedCommand
        {
            get
            {
                return new Command<TKCustomMapPin>((TKCustomMapPin pin) =>
                {
                    MapRegion = MapSpan.FromCenterAndRadius(SelectedPin.Position, Distance.FromKilometers(1));
                });
            }
        }

        /// <summary>
        /// Drag End bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Command<RoutePin> DragEndCommand
        {
            get
            {
                return new Command<RoutePin>(pin =>
                {
                    var routePin = pin as RoutePin;

                    if (routePin != null)
                    {
                        if (routePin.IsSource)
                        {
                            routePin.Route.Source = pin.Position;
                        }
                        else
                        {
                            routePin.Route.Destination = pin.Position;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Route clicked bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Command<TKRoute> RouteClickedCommand
        {
            get
            {
                return new Command<TKRoute>(async r =>
                {
                    var action = await Application.Current.MainPage.DisplayActionSheet(
                        "Route tapped",
                        "Cancel",
                        null,
                        "Show Instructions");

                    if (action == "Show Instructions")
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new HtmlInstructionsPage(r));
                    }
                });
            }
        }

        /// <summary>
        /// Callout clicked bound to the <see cref="TKCustomMap"/>
        /// </summary>
        public Command CalloutClickedCommand
        {
            get
            {
                return new Command<TKCustomMapPin>(async (TKCustomMapPin pin) =>
                {
                    var action = await Application.Current.MainPage.DisplayActionSheet(
                        "Callout clicked",
                        "Cancel",
                        "Remove Pin");

                    if (action == "Remove Pin")
                    {
                        var PinFound = new Pins(); 

                        var appt = await App.Database.database.GetWithChildrenAsync<Appointment>(appointment.ID);

                        Pins[] pins_ = new Pins[appt.PinList.Count];
                        pins_ = appt.PinList.ToArray();
                        for (int count = 0; count < appt.PinList.Count && (pins_[count].Latitude != pin.Position.Latitude && pins_[count].Longitude != pin.Position.Longitude); count++)
                        {
                            PinFound = pins_[count]; 
                        }
                        _pins.Remove(pin);
                        pinlist.Remove(PinFound); 
                        await App.Database.database.DeleteAsync(PinFound);
                        await App.Database.database.UpdateWithChildrenAsync(appt); 
                    }
                });
            }
        }

        public Command ClearMapCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var answer = await Application.Current.MainPage.DisplayAlert("Are you sure you want to clear all save Map Items?", null, "Yes", "No");
                    if (answer)
                    {
                        var apptUpdate = await App.Database.database.GetWithChildrenAsync<Appointment>(appointment.ID);
                        await App.Database.database.DeleteAllAsync(apptUpdate.PinList);
                        await App.Database.database.DeleteAllAsync(apptUpdate.CircleList);
                        pinlist.Clear();
                        circlelist.Clear(); 
                        _pins.Clear();
                        _circles.Clear();
                        _polygons.Clear(); 
                        await App.Database.database.UpdateWithChildrenAsync(apptUpdate);
                    }
                });
            }
        }
        /// <summary>
        /// Navigate to a new page to get route source/destination
        /// </summary>
        public Command AddRouteCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Routes == null) Routes = new ObservableCollection<TKRoute>();

                    var addRoutePage = new AddRoutePage(Routes, Pins, MapRegion);
                    Application.Current.MainPage.Navigation.PushAsync(addRoutePage);
                });
            }
        }

        /// <summary>
        /// Command when a route calculation finished
        /// </summary>
        public Command<TKRoute> RouteCalculationFinishedCommand
        {
            get
            {
                return new Command<TKRoute>(r =>
                {
                    // move to the bounds of the route
                    MapRegion = r.Bounds;
                });
            }
        }

        public Func<string, IEnumerable<TKCustomMapPin>, TKCustomMapPin> GetClusteredPin => (group, clusteredPins) =>
        {
            return null;
            //return new TKCustomMapPin
            //{
            //    DefaultPinColor = Color.Blue,
            //    Title = clusteredPins.Count().ToString(),
            //    ShowCallout = true
            //};
        };

        public SampleViewModel(double long_, double lat_, Appointment Appt)
        {
            longitude = long_;
            latitude = lat_;
            _mapCenter = new Position(lat_, long_);
            _pins = new ObservableCollection<TKCustomMapPin>();
            _circles = new ObservableCollection<TKCircle>();
            appointment = Appt;

            if (appointment.CircleList == null)
            {
                circlelist = new List<Circles>(); 
            }
            else
            {
                circlelist = appointment.CircleList; 
            }

            if(appointment.PinList == null)
            {
                pinlist = new List<Pins>();
            }
            else
            {
                pinlist = appointment.PinList; 
            }          
            
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Position GetDummyPosition()
        {
            return new Position(Random(51.6723432, 51.38494009999999), Random(0.148271, -0.3514683));
        }
        double Random(double min, double max)
        {
            return _random.NextDouble() * (max - min) + min;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Command LoadMapItems
        {
            get
            {
                return new Command(async () =>
                {
                    var curr_appt = await App.Database.GetItemAsync(appointment.ID);
                    var appt = await App.Database.database.GetWithChildrenAsync<Appointment>(curr_appt.ID);
                    double lat_ = 0, long_ = 0;

                    if(appt.PinList != null)
                    {
                        Pins[] pins_ = new Pins[appt.PinList.Count];
                        pins_ = appt.PinList.ToArray();
                        for (int count = 0; count < appt.PinList.Count; count++)
                        {
                            long_ = pins_[count].Longitude;
                            lat_ = pins_[count].Latitude;

                            var pin_ = new TKCustomMapPin
                            {
                                Position = new Position(lat_, long_),
                                Title = string.Format("Pin {0}, {1}", lat_, long_),
                                ShowCallout = true,
                                IsDraggable = true
                            };
                            _pins.Add(pin_);
                        }
                        int polyline_count = 0;
                        List<Position> polyline_position = new List<Position>(); 
                        for(int count = 0; count < appt.PinList.Count; count++)
                        {
                            if(pins_[count].isPolyline == true)
                            {
                                polyline_position.Add(new Position(pins_[count].Latitude, pins_[count].Longitude));
                                var pin = new TKCustomMapPin
                                {
                                    Position = new Position(pins_[count].Latitude, pins_[count].Longitude),
                                    Title = string.Format("Pin {0}, {1}", pins_[count].Latitude, pins_[count].Latitude),
                                    ShowCallout = false,
                                    IsDraggable = false,
                                    IsCalloutClickable = false,
                                    DefaultPinColor = Color.DarkBlue
                                };
                                _pins.Add(pin); 
                                polyline_count++; 
                            }
                        }
                        if(polyline_count >= 3)
                        {
                            var polygon_ = new TKPolygon
                            {
                                StrokeColor = Color.DarkBlue,
                                StrokeWidth = 10f,
                                Color = Color.FromRgba(0, 0, 0, 0),
                                Coordinates = polyline_position
                            };
                            _polygons.Add(polygon_); 
                        }
                    }
                    
                    if (appt.CircleList != null)
                    {
                        Circles[] circles_ = new Circles[appt.CircleList.Count];
                        circles_ = appt.CircleList.ToArray();

                        for (int count = 0; count < appt.CircleList.Count; count++)
                        {
                            long_ = circles_[count].Longitude;
                            lat_ = circles_[count].Latitude;

                            var circle = new TKCircle
                            {
                                Center = new Position(lat_, long_),
                                Radius = 30,
                                Color = Color.FromRgba(100, 0, 0, 80)
                            };
                            _circles.Add(circle);
                        }
                    }
                });
            }
        }
    }
}
