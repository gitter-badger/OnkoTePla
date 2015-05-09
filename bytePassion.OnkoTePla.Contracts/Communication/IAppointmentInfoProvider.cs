using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Contracts.Communication
{
	public interface IAppointmentInfoProvider
	{
		IReadOnlyList<Appointment> GetAppointments();
	}
}
