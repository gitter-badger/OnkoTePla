using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.Appointments;


namespace xIT.OnkoTePla.Contracts.Communication
{
	public interface IAppointmentInfoProvider
	{
		IReadOnlyList<Appointment> GetAppointments();
	}
}
