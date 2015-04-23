using System;
using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.Patients;


namespace xIT.OnkoTePla.Contracts.Communication
{
	public interface IPatientInfoProvider
	{
		event Action<Patient> NewPatientAdded;
		event Action<Patient> PatientDataChanged;
		
		IReadOnlyList<Patient> GetPatients();
	}
}
