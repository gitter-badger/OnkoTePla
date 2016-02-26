using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.JsonSerializationDoubles;
using Newtonsoft.Json.Serialization;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
	public class PracticeMetaData
    {
	    public DateTime FirstAppointmentDate { get; set; }
        public DateTime LastAppointmentDate { get; set; }

        public Dictionary<Guid, List<DateTime>> AppointmentsForPatient { get; set; } = new Dictionary<Guid, List<DateTime>>();

        public Dictionary<DateTime, short> AppointmentExistenceIndex { get; set; } = new Dictionary<DateTime, short>();
    }

    //public class PracticeMetaDataSerializationDouble
    //{

    //    public DateSerializationDouble FirstAppointmentDate { get; set; }
    //    public DateSerializationDouble LastAppointmentDate { get; set; }

    //    public Dictionary<Guid, List<DateSerializationDouble>> AppointmentsForPatient { get; set; }
    //    public Dictionary<DateSerializationDouble, short> AppointmentExistenceIndex { get; set; }  

    //    public PracticeMetaDataSerializationDouble(PracticeMetaData metaData)
    //    {
    //        FirstAppointmentDate = new DateSerializationDouble(metaData.FirstAppointmentDate);
    //        LastAppointmentDate = new DateSerializationDouble(metaData.LastAppointmentDate);
    //        AppointmentsForPatient = new Dictionary<Guid, List<DateSerializationDouble>>();
    //        AppointmentExistenceIndex = new Dictionary<DateSerializationDouble, short>();

    //        foreach (var entry in metaData.AppointmentsForPatient)
    //        {
    //            AppointmentsForPatient.Add(entry.Key, entry.Value.Select(d => new DateSerializationDouble(d)).ToList());
    //        }

    //        foreach (var entry in metaData.AppointmentExistenceIndex)
    //        {
    //            var key = new DateSerializationDouble(entry.Key);
    //            AppointmentExistenceIndex.Add(key, entry.Value);
    //        }

    //    }

    //    public PracticeMetaData GetPracticeMetaData() 
    //    {
    //        var metaData = new PracticeMetaData();
    //        metaData.FirstAppointmentDate = FirstAppointmentDate.GetDate();
    //        metaData.LastAppointmentDate = LastAppointmentDate.GetDate();

    //        foreach (var entry in AppointmentsForPatient)
    //        {
    //            metaData.AppointmentsForPatient.Add(entry.Key, entry.Value.Select( d => d.GetDate()).ToList());
    //        }

    //        foreach (var entry in AppointmentExistenceIndex)
    //        {
    //            metaData.AppointmentExistenceIndex.Add(entry.Key.GetDate(), entry.Value);
    //        }

    //        return metaData;
    //    }
    //}
}