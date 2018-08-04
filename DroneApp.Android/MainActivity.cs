
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin;
using Plugin.CurrentActivity;
namespace DroneApp.Droid
{
    [Activity(Label = "DroneApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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
            LoadApplication(new App ());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

