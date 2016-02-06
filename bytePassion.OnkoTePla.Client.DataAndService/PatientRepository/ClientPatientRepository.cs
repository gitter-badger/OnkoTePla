using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.PatientRepository
{
	internal class ClientPatientRepository : IClientPatientRepository
	{		
		private readonly IConnectionService connectionService;
		private readonly IDictionary<Guid, Patient> cachedPatients; 

		public ClientPatientRepository(IConnectionService connectionService)
		{
			this.connectionService = connectionService;
			cachedPatients = new ConcurrentDictionary<Guid, Patient>();
		}
		
		public Patient GetPatient(Guid id)
		{
			if (ArePatientsAvailable())
				if (cachedPatients.ContainsKey(id))
					return cachedPatients[id];

			return null;
		}

		public bool ArePatientsAvailable()
		{
			return cachedPatients != null;
		}

		public void RequestPatients(Action patientsAvailable, Action<string> errorCallback)
		{
			connectionService.RequestPatientList(
				patientList =>
				{
					foreach (var patient in patientList)
					{
						if (!IsPatientsAvailable(patient.Id))
							cachedPatients.Add(patient.Id, patient);
					}

					patientsAvailable();
				},
				errorCallback
			);
		}
		
		public bool IsPatientsAvailable(Guid id)
		{
			return cachedPatients.ContainsKey(id);
		}					
	}
}