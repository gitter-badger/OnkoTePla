using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamMetaData;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.XmlDataStores
{
	public class XmlPracticeMetaDataPersistanceService : IPersistenceService<IEnumerable<IPracticeMetaData>>
	{

		private static XmlWriterSettings WriterSettings
		{
			get
			{
				return new XmlWriterSettings
				{
					Indent = true,
					IndentChars = "  ",
					NewLineChars = "\r\n",
					NewLineHandling = NewLineHandling.Replace
				};
			}
		}

		private const string XmlRoot                       = "metaDataFiles";

		private const string MetaData                      = "metaData";
		private const string AppointmentsForPatient        = "appointmentsForPatient";
		private const string AppointmentExistanceIndex     = "appointmentExistingIndex";
		private const string PatientInfo                   = "patientInfo";
		private const string PatientDate                   = "patientDate";
		private const string ExistanceInfo                 = "existanceInfo";

		private const string FirstAppointmentDataAttribute = "firstAppointmentDate";
		private const string LastAppointmentDataAttribute  = "lastAppointmentDate";
		private const string MedicalPracticeIdAttribute    = "medicalPracticeId";
		private const string PatientIdAttribute            = "patientId";
		private const string DateAttribute                 = "date";
		private const string CountAttribute                = "count";

		private readonly string filename;

		public XmlPracticeMetaDataPersistanceService(string filename)
		{
			this.filename = filename;
		}
		
		public void Persist(IEnumerable<IPracticeMetaData> metaDataFiles)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();

			writer.WriteStartElement(XmlRoot);

			foreach (var metaData in metaDataFiles)
			{
				WriteMetaData(writer, metaData);				
			}

			writer.WriteEndElement();

			writer.WriteEndDocument();
			writer.Close();
		}

		public void WriteMetaData(XmlWriter writer, IPracticeMetaData metaData)
		{
			writer.WriteStartElement(MetaData);

			writer.WriteAttributeString(FirstAppointmentDataAttribute, metaData.FirstAppointmentDate.ToString());
			writer.WriteAttributeString(LastAppointmentDataAttribute,  metaData.LastAppointmentDate.ToString());
			writer.WriteAttributeString(MedicalPracticeIdAttribute,    metaData.MedicalPracticeId.ToString());

			WriteAppointmentsForPatient(writer, metaData.AppointmentsForPatient);
			WriteAppointmentExistanceIndex(writer, metaData.AppointmentExistenceIndex);

			writer.WriteEndElement();
		}

		private void WriteAppointmentExistanceIndex(XmlWriter writer, IDictionary<Date, ushort> appointmentExistenceIndex)
		{
			writer.WriteStartElement(AppointmentExistanceIndex);

			writer.WriteAttributeString(CountAttribute, appointmentExistenceIndex.Count.ToString());

			foreach (var existanceInfo in appointmentExistenceIndex)
			{
				writer.WriteStartElement(ExistanceInfo);
				writer.WriteAttributeString(DateAttribute, existanceInfo.Key.ToString());
				writer.WriteAttributeString(CountAttribute, existanceInfo.Value.ToString());
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		public void WriteAppointmentsForPatient(XmlWriter writer, IDictionary<Guid, IList<Date>> appointmentsForPatient)
		{
			writer.WriteStartElement(AppointmentsForPatient);

			writer.WriteAttributeString(CountAttribute, appointmentsForPatient.Count.ToString());

			foreach (var patientInfo in appointmentsForPatient)
			{
				WritePatientInfo(writer, patientInfo);
			}

			writer.WriteEndElement();
		}

		private void WritePatientInfo(XmlWriter writer, KeyValuePair<Guid, IList<Date>> patientInfo)
		{
			writer.WriteStartElement(PatientInfo);

			writer.WriteAttributeString(PatientIdAttribute, patientInfo.Key.ToString());
			writer.WriteAttributeString(CountAttribute,     patientInfo.Value.Count.ToString());

			foreach (var date in patientInfo.Value)
			{
				writer.WriteStartElement(PatientDate);
				writer.WriteAttributeString(DateAttribute, date.ToString());
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		public IEnumerable<IPracticeMetaData> Load()
		{
			IList<IPracticeMetaData> metaDataFiles = new List<IPracticeMetaData>();

			if (!File.Exists(filename))
				return metaDataFiles;

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				if (reader.IsEmptyElement) return metaDataFiles;

				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != MetaData) continue;
					if (!reader.HasAttributes) continue;
					
					var firstAppointmentDate = Date.Dummy;
					var lastAppointmentDate  = Date.Dummy;				
					var medicalPracticeId    = new Guid();

					while (reader.MoveToNextAttribute())
					{
						switch (reader.Name)
						{
							case FirstAppointmentDataAttribute: firstAppointmentDate = Date.Parse(reader.Value); break;
							case LastAppointmentDataAttribute:  lastAppointmentDate  = Date.Parse(reader.Value); break;
							case MedicalPracticeIdAttribute:    medicalPracticeId    = Guid.Parse(reader.Value); break;
						}
					}

					var appointmentForPatients = AcceptAppointmentsForPatient(reader);
					var appointmentExistanceIndex = AcceptAppointmentExistanceIndex(reader);

					metaDataFiles.Add(new PracticeMetaData(medicalPracticeId, firstAppointmentDate, lastAppointmentDate, 
														   appointmentForPatients, appointmentExistanceIndex));					
				}
			}
			reader.Close();

			return metaDataFiles;
		}

		private static IDictionary<Guid, IList<Date>> AcceptAppointmentsForPatient (XmlReader reader)
		{			
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.Name == AppointmentsForPatient)
					{
						var itemCount = 0;
						
						while (reader.MoveToNextAttribute())
						{
							switch (reader.Name)
							{
								case CountAttribute: itemCount = int.Parse(reader.Value); break;							
							}
						}

						return AcceptPatientInfoList(reader, itemCount);
					}
					
					break;
				}
			}

			throw new XmlException();
		}

		private static IDictionary<Guid, IList<Date>> AcceptPatientInfoList (XmlReader reader, int itemsCount)
		{
			var infoList = new List<Tuple<Guid, IList<Date>>>();

			int i = 0;
			while (i < itemsCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != PatientInfo) continue;
				i++;

				var patientId = Guid.Empty;
				var dateCount = 0;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == PatientIdAttribute) patientId = Guid.Parse(reader.Value);
						if (reader.Name == CountAttribute)     dateCount = int.Parse(reader.Value);						
					}
				}

				var dateList = AcceptDateList(reader, dateCount);

				infoList.Add(new Tuple<Guid, IList<Date>>(patientId, dateList));			
			}

			return infoList.ToDictionary(item => item.Item1, item => item.Item2);
		}

		private static IList<Date> AcceptDateList(XmlReader reader, int listItemCount)
		{
			var dateList = new List<Date>();

			int i = 0;
			while (i < listItemCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != PatientDate) continue;
				i++;

				var date = Date.Dummy;				

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == DateAttribute) date = Date.Parse(reader.Value);						
					}
				}
				
				dateList.Add(date);
			}

			return dateList;			
		}

		private static IDictionary<Date, ushort> AcceptAppointmentExistanceIndex(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.Name == AppointmentExistanceIndex)
					{
						var itemCount = 0;

						while (reader.MoveToNextAttribute())
						{
							switch (reader.Name)
							{
							case CountAttribute: itemCount = int.Parse(reader.Value); break;
							}
						}

						return AcceptExistanceInfoList(reader, itemCount);
					}

					break;
				}
			}

			throw new XmlException();			
		}

		private static IDictionary<Date, ushort> AcceptExistanceInfoList(XmlReader reader, int itemCount)
		{
			var infoList = new List<Tuple<Date, ushort>>();

			int i = 0;
			while (i < itemCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != ExistanceInfo) continue;
				i++;

				var date  = Date.Dummy;
				ushort count = 0;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == DateAttribute)  date  = Date.Parse(reader.Value);
						if (reader.Name == CountAttribute) count = ushort.Parse(reader.Value);
					}
				}				

				infoList.Add(new Tuple<Date, ushort>(date, count));
			}

			return infoList.ToDictionary(item => item.Item1, item => item.Item2);
		} 
	}
}
