using DroneApp.DataBase;
using SQLite;
using SQLiteNetExtensions.Attributes;
using TK.CustomMap;
using TK.CustomMap.Overlays;

namespace DroneApp.TKMap.CustomPins
{
    /// <summary>
    /// Pin which is either source or destination of a route
    /// </summary>
    public class RoutePin : TKCustomMapPin
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [ForeignKey(typeof(Appointment))]
        public int Appt_ID { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        public Appointment Appt { get; set; }
        /// <summary>
        /// Gets/Sets if the pin is the source of a route. If <value>false</value> pin is destination
        /// </summary>
        public bool IsSource { get; set; }
        public TKRoute Route { get; internal set; }
    }
}
