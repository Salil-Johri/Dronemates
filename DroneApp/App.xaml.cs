using DroneApp.DataBase;
using System;
using System.IO;
using Xamarin.Forms;
using DroneApp.MainPageAppt;
using TK.CustomMap.Api.Google;

namespace DroneApp
{
    public partial class App : Application
	{
        public static double ScreenHeight;
        public static double ScreenWidth;

        static AppointmentDatabase database;
        public App ()
		{
            InitializeComponent();

            GmsPlace.Init("AIzaSyCr2z_PnUvCFuKNnt_UAGed0jaMWWhDpz0");
            GmsDirection.Init("AIzaSyCr2z_PnUvCFuKNnt_UAGed0jaMWWhDpz0");

            MainPage = new MasterAppointment();
		}
        public static AppointmentDatabase Database
        {
            get
            {
                if (database == null)
                {
                    //Create file path
                    database = new AppointmentDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "plswork.db3")); //shit
                }
                return database;
            }
        }
     
        public int ResumeAtTodoId { get; set; }

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
