using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.Patients
{
	public interface IPatientReadRepository : IPersistable
	{
		event Action<Patient> PatientAdded;
		event Action<Patient> PatientModified;

		Patient GetPatientById(Guid id);
		IEnumerable<Patient> GetAllPatients();
	}
	
}
