using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository
{
	public class ClientPatientRepository : IClientPatientRepository
	{		
		private readonly IConnectionService connectionService;
		private  IDictionary<Guid, Patient> cachedPatients; 

		public ClientPatientRepository(IConnectionService connectionService)
		{
			this.connectionService = connectionService;			
		}
				
		public void RequestPatient(Action<Patient> patientAvailableCallback, Guid patientId, Action<string> errorCallback)
		{
			if (cachedPatients == null)
			{
				connectionService.RequestPatientList(
					patientList =>
					{
						cachedPatients = new Dictionary<Guid, Patient>();
						foreach (var patient in patientList)
						{
							cachedPatients.Add(patient.Id, patient);
						}

						patientAvailableCallback(GetPatient(patientId, errorCallback));
					},
					errorCallback
				);
			}
			else
			{
				patientAvailableCallback(GetPatient(patientId, errorCallback));
			}
		}
		
		public void RequestAllPatientList(Action<IReadOnlyList<Patient>> patientListAvailableCallback, Action<string> errorCallback)
		{
			if (cachedPatients == null)
			{
				connectionService.RequestPatientList(
					patientList =>
					{
						cachedPatients = new Dictionary<Guid, Patient>();
						foreach (var patient in patientList)
						{
							cachedPatients.Add(patient.Id, patient);
						}

						patientListAvailableCallback(cachedPatients.Values.ToList());
					},
					errorCallback
				);
			}
			else
			{
				patientListAvailableCallback(cachedPatients.Values.ToList());
			}
		}

		private Patient GetPatient (Guid id, Action<string> errorCallback)
		{			
			if (cachedPatients.ContainsKey(id))
				return cachedPatients[id];

			errorCallback("patient not found");
			return null;
		}
	}
}