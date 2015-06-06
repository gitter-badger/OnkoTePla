using System;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Patients
{
	public interface IPatientReadRepository
	{
		event Action<Contracts.Patients.Patient> PatientAdded;
		event Action<Contracts.Patients.Patient> PatientModified;

		Contracts.Patients.Patient GetPatientById(Guid id);
	}
	
}
