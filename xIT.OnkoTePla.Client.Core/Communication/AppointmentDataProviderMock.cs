using System.Collections.Generic;
using xIT.OnkoTePla.Contracts;
using xIT.OnkoTePla.Contracts.Appointments;
using xIT.OnkoTePla.Contracts.Communication;


namespace xIT.OnkoTePla.Client.Core.Communication
{
	public class AppointmentDataProviderMock : IAppointmentInfoProvider
	{
		public IReadOnlyList<Appointment> GetAppointments()
		{
			return CommunicationSampleData.Appointments;
		}
	}
}
