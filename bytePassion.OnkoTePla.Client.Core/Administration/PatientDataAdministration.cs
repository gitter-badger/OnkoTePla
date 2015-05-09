using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Administration
{
	public class PatientDataAdministration
	{
		private IDictionary<uint, Patient> patients; 

		public PatientDataAdministration(IEnumerable<Patient> initialPatients)
		{			
			patients = initialPatients.ToDictionary(patient => patient.ID, patient => patient);			
		}

		public Patient this[uint patientID]
		{
			get
			{
				return patients.ContainsKey(patientID) ? patients[patientID] : null;
			}
		}
	}
}
