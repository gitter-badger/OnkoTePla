using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class AppointmentsOfADay
	{
		//public event EventHandler<AppointmentChangedEventArgs> AppointmentChanged; 

		private readonly IReadOnlyList<Appointment> appointments;

		public AppointmentsOfADay(IReadOnlyList<Appointment> initialStateOfDay)
		{
			appointments = initialStateOfDay;			
		}

		public IReadOnlyList<Appointment> Appointments { get { return appointments; } } 
	}
}
