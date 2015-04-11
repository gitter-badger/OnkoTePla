using System;
using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.Appointments;
using xIT.OnkoTePla.Contracts.Communication;


namespace xIT.OnkoTePla.Client.Core.Communication
{
	public class AppointmentDataProviderMock : IAppointmentInfoProvider
	{
		public IReadOnlyList<Appointment> GetAppointments()
		{
			var listOfPatients = new PatientDataProviderMock().GetPatients();
			var listOfTherapyPlaces = new MedicalPracticeInfoProviderMock().GetMedicalPractice().AllTherapyPlaces;

			return new List<Appointment>()
			{
				new Appointment(listOfPatients[0], listOfTherapyPlaces[0], new DateTime(2015, 4, 16, 10, 15, 00), new DateTime(2015, 4, 16, 11, 30, 00)),
				new Appointment(listOfPatients[0], listOfTherapyPlaces[3], new DateTime(2015, 4, 20, 10, 15, 00), new DateTime(2015, 4, 20, 11, 30, 00)),
				new Appointment(listOfPatients[1], listOfTherapyPlaces[1], new DateTime(2015, 4, 17, 11, 00, 00), new DateTime(2015, 4, 17, 12, 30, 00)),
				new Appointment(listOfPatients[2], listOfTherapyPlaces[1], new DateTime(2015, 4, 21,  9, 00, 00), new DateTime(2015, 4, 21, 10, 00, 00)),
				new Appointment(listOfPatients[2], listOfTherapyPlaces[4], new DateTime(2015, 4, 16, 15, 30, 00), new DateTime(2015, 4, 16, 16, 30, 00)),
				new Appointment(listOfPatients[3], listOfTherapyPlaces[2], new DateTime(2015, 4, 16, 10, 15, 00), new DateTime(2015, 4, 16, 11, 30, 00))
			};
		}
	}
}
