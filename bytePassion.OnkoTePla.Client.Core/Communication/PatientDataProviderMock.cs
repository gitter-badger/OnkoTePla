using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts;
using bytePassion.OnkoTePla.Contracts.Communication;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Communication
{
	public class PatientDataProviderMock : IPatientInfoProvider
	{
		public event Action<Patient> NewPatientAdded;
		public event Action<Patient> PatientDataChanged;

		public IReadOnlyList<Patient> GetPatients()
		{
			return CommunicationSampleData.PatientList;
		}
	}
}
