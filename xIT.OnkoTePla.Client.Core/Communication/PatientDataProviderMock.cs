using System;
using System.Collections.Generic;
using xIT.OnkoTePla.Contracts;
using xIT.OnkoTePla.Contracts.Communication;
using xIT.OnkoTePla.Contracts.Patients;


namespace xIT.OnkoTePla.Client.Core.Communication
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
