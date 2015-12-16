using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Patients;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Core.Repositories.Patients
{
    public interface IPatientReadRepository : IPersistable
	{
		event Action<Patient> PatientAdded;
		event Action<Patient> PatientModified;

		Patient GetPatientById(Guid id);
		IEnumerable<Patient> GetAllPatients();
	}
	
}
