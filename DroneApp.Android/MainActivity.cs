using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin;
using Plugin.CurrentActivity;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TK.CustomMap.Droid;

namespace DroneApp.Droid
{
    [Activity(Label = "Dronemates", Icon = "@drawable/DronematesLogo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            Instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            FormsGoogleMaps.Init(this, bundle); 
            FormsMaps.Init(this, bundle);
            TKGoogleMaps.Init(this, bundle);
            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;

            LoadApplication(new App());

            AppCenter.Start("66fd97a2-5b59-45a3-afdf-1af54967204a",
                   typeof(Analytics), typeof(Crashes));
            AppCenter.Start("66fd97a2-5b59-45a3-afdf-1af54967204a", typeof(Analytics), typeof(Crashes));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

