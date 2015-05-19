using System;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands
{
	public class AddAppointment : DomainCommand
	{
		private readonly Appointment newAppointment;

		public AddAppointment(Guid aggregateId, int aggregateVersion, Appointment newAppointment) 
			: base(aggregateId, aggregateVersion)
		{
			this.newAppointment = newAppointment;
		}

		public Appointment NewAppointment
		{
			get { return newAppointment; }
		}
	}
}
