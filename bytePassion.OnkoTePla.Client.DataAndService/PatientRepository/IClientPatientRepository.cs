using System;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.PatientRepository
{
	public interface IClientPatientRepository
	{				
		Patient GetPatient(Guid id);
		bool ArePatientsAvailable();
		void RequestPatients(Action patientsAvailable, Action<string> errorCallback);
	}
}
