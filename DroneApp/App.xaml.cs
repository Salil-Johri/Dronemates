using DroneApp.DataBase;
using System;
using System.IO;
using Xamarin.Forms;
using DroneApp.MainPageAppt;
namespace DroneApp
{
    public partial class App : Application
	{
        static AppointmentDatabase database;
        public App ()
		{
            InitializeComponent();
            MainPage = new MasterAppointment();
		}
        public static AppointmentDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new AppointmentDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoSQLite.db3"));
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
