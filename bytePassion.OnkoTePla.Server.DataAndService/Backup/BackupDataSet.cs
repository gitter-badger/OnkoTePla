using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Server.DataAndService.Backup
{
	internal class BackupDataSet
	{
		public BackupDataSet(IEnumerable<Patient> patientData, 
							 IEnumerable<string> eventHistoryFiles, 
							 Configuration configurationData)
		{
			PatientData = patientData;
			EventHistoryFiles = eventHistoryFiles;
			ConfigurationData = configurationData;
		}

		public IEnumerable<Patient> PatientData       { get; } 
		public IEnumerable<string>  EventHistoryFiles { get; } 
		public Configuration		ConfigurationData { get; }
	}
}
