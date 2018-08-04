using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneApp.MainPageAppt
{

    public class MasterAppointmentMenuItem
    {
        public MasterAppointmentMenuItem()
        {
            TargetType = typeof(MasterAppointmentDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}