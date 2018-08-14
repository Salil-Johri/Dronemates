using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace DroneApp.DataBase
{
    public class Appointment
    {
        // Main Appointment object, acts as object that saves all information in app. 
        // this object is saved to database, along with children data types

        /* Primarykey is for database, which assigns key to an instance of the object which is used to access this object. 
         * AutoIncrement automatically increments the key value so that it does not have to be done manually.
         */
        [PrimaryKey, AutoIncrement]
        /* These data types are for the overarching "appointment", and is basic information of the appointment, such as lat and long for 
         * the location, whether the appointment is done or not, etc. 
         */
        public int ID { get; set; }
        public int Done { get; set; }
        public DateTime StartDateJob { get; set; }
        public DateTime EndDateJob { get; set; }
        public TimeSpan StartJobTime { get; set; }
        public TimeSpan EndJobTime { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        // This list of objects is used for the Aerodromes page to display aerodromes. 
        // The OneToMany relationship allows the list to be saved as a child of the Appointment object
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Aerodromes> Given_Aerodromes { get; set; }

        // This list of objects is used in the map, to save a list of pins that the user makes.
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Pins> PinList { get; set; }


        // This list of objects is used in the map page, to save a list of circles that the user makes on app.
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Circles> CircleList { get; set; }

        // These variables are for the Site prep page, and are used to save the information inputted into this page. 
        public string DroneName { get; set; }
        public string OwnerPerm { get; set; }
        public string UAVType { get; set; }
        public string Airspace { get; set; }
        public string ContactName { get; set; }
        public string ACCMan { get; set; }
        public string ACCNumber { get; set; }
        public string UAVColour { get; set; }
        public string ContactNum { get; set; }
        public string FireNum { get; set; }
        public string PoliceNum { get; set; }
        public string NotamNum { get; set; }
        public string AmbNum { get; set; }
        public double MaxAlt { get; set; }
        public double Radius { get; set; }
        public string Wingspan { get; set; }
        public string UAVWeight { get; set; }
        public string NotamPhone { get; set; }
        public int degreesLat { get; set; }
        public int minutesLat { get; set; }
        public double secondsLat { get; set; }
        public string directionLat { get; set; }
        public int degreesLong { get; set; }
        public int minutesLong { get; set; }
        public double secondsLong { get; set; }
        public string directionLong { get; set; }
        public DateTime DateNotamStart { get; set; }
        public DateTime DateNotamEnd { get; set; }
        public double RadiusNM { get; set; }
        public string HourStart { get; set; }
        public string MinStart { get; set; }
        public string HourEnd { get; set; }
        public string MinEnd { get; set; }


        // These variables are for the Normal prep page, and are used to save the information inputted into this page.
        public string AccCalled { get; set; }
        public string Damage2Drone { get; set; }
        public string BatteriesCharged { get; set; }
        public string PropsMounted { get; set; }
        public string CameraMounted { get; set; }
        public string ControllerFunctional { get; set; }
        public string GoConnected { get; set; }
        public string CompassCalibrated { get; set; }
        public string FlightHeight { get; set; }
        public string SDCard { get; set; }
        public string Dist { get; set; }
        public string HomeAlt { get; set; }
        public string LowBatt { get; set; }
        public string CritBatt { get; set; }
        public string flight_mode { get; set; }
        public string home_point_accuracy { get; set; }
        public string transmission_interference { get; set; }
        public string current_functionality { get; set; }
        public string description_if_no { get; set; }
        public string can_project_go { get; set; }
        public string MultFlight { get; set; }


        //  These variables are for the Morning prep page, and are used to save the information inputted into this page. 
        public string batteries_remote { get; set; }
        public string batteries_intelligent_flight { get; set; }
        public string batteries_mobile_device { get; set; }
        public string weather_visibility { get; set; }
        public string cloud_ceiling { get; set; }
        public string wind_gusts { get; set; }
        public string adverse_weather { get; set; }
        public string temperature { get; set; }
        public string radio_license { get; set; }
        public string flight_certificate { get; set; }
        public string sfoc_certification { get; set; }
        public string insurance_policy { get; set; }
        public string tor_vta { get; set; }
        public string drone_manuals { get; set; }
        public string dji_working { get; set; }

        //  These variables are for the Security prep page, and are used to save the information inputted into this page. 
        public string home_point_confirm { get; set; }
        public string pylons_around_site { get; set; }
        public string fire_extinguisher { get; set; }
        public string first_aid_kit { get; set; }
        public string ground_supervisor { get; set; }
        public string aviation_radio { get; set; }
        public string necessary_docs { get; set; }
        public string site_clear { get; set; }
        public string unnecessary_equip_stored { get; set; }
        public string distance_public_maintained { get; set; }
    }
}
