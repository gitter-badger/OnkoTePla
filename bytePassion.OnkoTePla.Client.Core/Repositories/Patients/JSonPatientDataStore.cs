using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Patients
{
	public class JSonPatientDataStore :  IPersistenceService<IEnumerable<Patient>>
	{
		
		private readonly string filename;

		public JSonPatientDataStore(string filename)
		{
			this.filename = filename;
		}

		public void Persist(IEnumerable<Patient> data)
		{
			// TODO: persist here
		}

		public IEnumerable<Patient> Load()
		{
			// TODO load here
			return new List<Patient>();
		}
	}
}
