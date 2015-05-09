using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Communication;


namespace bytePassion.OnkoTePla.Client.Core.Communication
{
	public class AppointmentDataProviderMock : IAppointmentInfoProvider
	{
		public IReadOnlyList<Appointment> GetAppointments()
		{
			return CommunicationSampleData.Appointments;
		}
	}
}
