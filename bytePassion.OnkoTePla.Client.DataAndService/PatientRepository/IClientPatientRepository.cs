using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.PatientRepository
{
	public interface IClientPatientRepository
	{						
		void RequestPatient(Action<Patient> patientAvailableCallback, Guid patientId, Action<string> errorCallback);
		void RequestAllPatientList(Action<IReadOnlyList<Patient>> patientListAvailableCallback, Action<string> errorCallback);
	}
}
