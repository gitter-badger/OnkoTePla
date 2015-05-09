using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Contracts.Communication
{
	public interface IPatientInfoProvider
	{
		event Action<Patient> NewPatientAdded;
		event Action<Patient> PatientDataChanged;
		
		IReadOnlyList<Patient> GetPatients();
	}
}
