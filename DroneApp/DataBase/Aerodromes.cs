using SQLite;
using SQLiteNetExtensions.Attributes;
using DroneApp.DataBase; 
namespace DroneApp
{
    public class Aerodromes
    {        
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string AeroCode { get; set; }
        public string AeroName { get; set; }
        public string AeroDistance { get; set; }
        public string AeroDirection { get; set; }
        public string AeroPhone { get; set; }
        public string AeroATF { get; set; }

        [ForeignKey(typeof(Appointment))]
        public int Appointment_ID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Appointment Appt { get; set; }
    }
}