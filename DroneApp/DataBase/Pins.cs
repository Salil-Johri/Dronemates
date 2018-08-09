using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace DroneApp.DataBase
{
    public class Pins
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ForeignKey(typeof(Appointment))]
        public int Appt_ID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Appointment Appt { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool isPolyline { get; set; }
    }
}
