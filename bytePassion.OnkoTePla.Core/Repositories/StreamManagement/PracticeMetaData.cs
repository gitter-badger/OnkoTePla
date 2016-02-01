using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public class PracticeMetaData
    {
        public Date FirstAppointmentDate { get; set; }
        public Date LastAppointmentDate { get; set; }

        public Dictionary<Guid, List<Date>> AppointmentsForPatient { get; set; }
        public Dictionary<Date, bool> AppointmentExistenceIndex { get; set; } 
    }
}