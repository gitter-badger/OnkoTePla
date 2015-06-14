using System;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Patients
{
	public interface IPatientWriteRepository : IPersistable
	{
		void AddPatient(string name, Date birthday, bool alive);

		void SetNewName(Guid patientId, string newName);
		void SetNewBirthday(Guid patientId, Date newBirthday);
		void SetLivingStatus(Guid patientId, bool newLivingStatus);
	}
}