using System;
using System.Globalization;
using System.IO;
using System.Xml;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Server.DataAndService.Backup;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.LocalSettings;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.XmlDataStores
{
	public class LocalSettingsXmlPersistenceService : IPersistenceService<LocalSettingsData>
	{
		private readonly string filename;

		public LocalSettingsXmlPersistenceService(string filename)
		{
			this.filename = filename;
		}

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

		private const string XmlRoot        = "localSettings";
		private const string BackupSchedule = "backupSchedule";


		private const string BackupIntervalAttribute  = "backupInterval";
		private const string BackupDirectoryAttribute = "backupDirectory";
		private const string BackupTimeAttribute      = "backupTime";
		private const string BackupWeekDayAttribute   = "backupWeekDay";
		private const string BackupDayAttribute       = "backupDay";		
		private const string LastBackupAttribute      = "LastBackup";		

		public void Persist(LocalSettingsData data)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();

			writer.WriteStartElement(XmlRoot);

			WriteBackupSchedule(writer, data);

			writer.WriteEndElement();

			writer.WriteEndDocument();
			writer.Close();
		}

		private static void WriteBackupSchedule(XmlWriter writer, LocalSettingsData data)
		{
			writer.WriteStartElement(BackupSchedule);

			writer.WriteAttributeString(BackupIntervalAttribute,  data.BackupInterval.ToString());
			writer.WriteAttributeString(BackupDirectoryAttribute, data.BackupDirectory);
			writer.WriteAttributeString(BackupTimeAttribute,      data.BackupTime.ToString());
			writer.WriteAttributeString(BackupWeekDayAttribute,   data.BackupWeekDay.ToString());
			writer.WriteAttributeString(BackupDayAttribute,       data.BackUpDay.ToString());			
			writer.WriteAttributeString(LastBackupAttribute,      data.LastBackup.ToString(CultureInfo.InvariantCulture));

			writer.WriteEndElement();
		}		

		public LocalSettingsData Load()
		{
			if (!File.Exists(filename))
				return LocalSettingsData.CreateDefaultSettings();

			var backupInterval  = BackupInterval.None;
			var backupDirectory = string.Empty;
			var backupTime      = Time.Dummy;
			var backUpWeekDay   = DayOfWeek.Monday;
			var backupDay       = -1;			
			var lastBackup      = DateTime.MinValue;

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element) continue;

					switch (reader.Name)
					{
						case BackupSchedule:
						{
							while (reader.MoveToNextAttribute())
							{
								switch (reader.Name)
								{
									case BackupIntervalAttribute:  backupInterval  = (BackupInterval)Enum.Parse(typeof(BackupInterval), reader.Value); break;
									case BackupDirectoryAttribute: backupDirectory = reader.Value;                                                     break;
									case BackupTimeAttribute:      backupTime      = Time.Parse(reader.Value);                                         break;
									case BackupWeekDayAttribute:   backUpWeekDay   = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), reader.Value);           break;
									case BackupDayAttribute:       backupDay       = int.Parse(reader.Value);                                          break;									                                      
									case LastBackupAttribute:      lastBackup      = DateTime.Parse(reader.Value);                                     break;
								}
							}
							break;
						}

//						case LastSession:
//						{
//							while (reader.MoveToNextAttribute())
//							{
//								switch (reader.Name)
//								{								
//								}
//							}
//							break;							
//						}
					}										
				}
			}
			reader.Close();

			return new LocalSettingsData(backupInterval, backupDirectory, 
										 backupTime, backUpWeekDay, backupDay,
										 lastBackup);
		}
	}
}
