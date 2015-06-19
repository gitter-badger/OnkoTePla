using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Patients
{
	public class XmlPatientDataStore_test :  IPersistenceService<IEnumerable<Patient>>
	{
		[Serializable]
		[XmlRoot("patientsData")]
		public class PatientsSerializable
		{
			[XmlArray("patients")]
			[XmlArrayItem("patient", typeof(PatientDtoSerializable))]			
			public IEnumerable<PatientDtoSerializable> Patients { get; set; }

			public static PatientsSerializable CreateFromPatientList(IEnumerable<Patient> patients)
			{
				return new PatientsSerializable()
				       {
					       Patients = patients.Select(PatientDtoSerializable.CreateFromPatient)
				       };
			}
		}

		[Serializable]
		//[XmlRoot("patient")]
		public class PatientDtoSerializable 
		{
			[XmlAttribute("name")]       public string            Name       { get; set; }
			[XmlAttribute("alive")]      public bool              Alive      { get; set; }
			[XmlElement  ("birthday")]   public DateSerializable  Birthday   { get; set; }
			[XmlElement  ("id")]         public Guid              Id         { get; set; }
			[XmlAttribute("externalId")] public string            ExternalId { get; set; }

			public static PatientDtoSerializable CreateFromPatient(Patient p)
			{
				return new PatientDtoSerializable()
				       {
					       Name = p.Name,
						   Alive = p.Alive,
						   Birthday = DateSerializable.CreateFromDate(p.Birthday),
						   Id = p.Id,
						   ExternalId = p.ExternalId
				       };
			}
		}

		[Serializable]
		//[XmlRoot("date")]
		public class DateSerializable
		{
			[XmlAttribute("day")]   public byte   Day   { get; set; }
			[XmlAttribute("month")] public byte   Month { get; set; }
			[XmlAttribute("year")]  public ushort Year  { get; set; }

			public static DateSerializable CreateFromDate(Date d)
			{
				return new DateSerializable()
				       {
					       Day = d.Day,
						   Month = d.Month,
						   Year = d.Year
				       };
			}
		}


		private readonly string filename;

		public XmlPatientDataStore_test(string filename)
		{
			this.filename = filename;
		}

		public void Persist(IEnumerable<Patient> data)
		{
//			var patientsForSerialization = PatientsSerializable.CreateFromPatientList(data);
//
//			var xmlSerializer = new XmlSerializer(typeof (PatientsSerializable));
//			var fileWriteStream = new StreamWriter(filename);
//			xmlSerializer.Serialize(fileWriteStream, patientsForSerialization);
//			fileWriteStream.Close();

			var fileWriteStream = new StreamWriter(filename);
			var xmlSerializer = new XmlSerializer(typeof(PatientDtoSerializable));

			var exampleDate = new DateSerializable {Day = 1, Month = 2, Year = 2000};
			var examplePatient = new PatientDtoSerializable
			                     {
				                     Alive = true,
				                     Birthday = exampleDate,
				                     Id = Guid.NewGuid(),
				                     ExternalId = "1"
			                     };

			xmlSerializer.Serialize(fileWriteStream, examplePatient);

			fileWriteStream.Close();
		}

		public IEnumerable<Patient> Load()
		{
			throw new NotImplementedException();
		}
	}
}
