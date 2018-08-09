using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace DroneApp.DataBase
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
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
        public int SaveCount { get; set; }

        //Aerodrome Page 
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Aerodromes> Given_Aerodromes { get; set; }

        //Pins 
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Pins> PinList { get; set; }
        //Circles
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Circles> CircleList { get; set; }

        // site prep
        public string DroneName { get; set; }
        public string OwnerPerm { get; set; }
        public string Location { get; set; }
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
        public string AirportName { get; set; }
        public string AirportCode { get; set; }
        public double MaxAlt { get; set; }
        public double Radius { get; set; }
        public string Wingspan { get; set; }
        public string UAVWeight { get; set; }
        public string NotamPhone { get; set; }
        public string DegCoord { get; set; }
        public string MinCoord { get; set; }
        public string SecCoord { get; set; }
        public string DegCoordLaunch { get; set; }
        public string MinCoordLaunch { get; set; }
        public string SecCoordLaunch { get; set; }
        public string DegCoordNotam { get; set; }
        public string MinCoordNotam { get; set; }
        public string SecCoordNotam { get; set; }
        public string DirectionNotam { get; set; }
        // NORMAL PREP PAGE
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
        // MORNING PREP PAGE
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
        // SECURITY PREP PAGE
        // Q1
        public string home_point_confirm { get; set; }
        // Q2
        public string pylons_around_site { get; set; }
        // Q3
        public string fire_extinguisher { get; set; }
        public string first_aid_kit { get; set; }
        public string ground_supervisor { get; set; }
        public string aviation_radio { get; set; }
        public string necessary_docs { get; set; }
        // Q4
        public string site_clear { get; set; }
        public string unnecessary_equip_stored { get; set; }
        // Q5
        public string distance_public_maintained { get; set; }
    }
}
