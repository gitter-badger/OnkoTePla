using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class AppointmentsOfADay : IDomainEventHandler<AppointmentAdded>
	{

		//public event EventHandler<AppointmentChangedEventArgs> AppointmentChanged; 


		private readonly IList<Appointment> appointments;

		public AppointmentsOfADay(IEnumerable<Appointment> initialStateOfDay)
		{
			appointments = initialStateOfDay.ToList();			
		}

		public IEnumerable<Appointment> Appointments { get { return appointments; } }

		public void Handle(AppointmentAdded domainEvent)
		{
			throw new System.NotImplementedException();
		}
	}
}
