using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;


namespace bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler
{
	public class AddAppointmentCommandHandler : IDomainCommandHandler<AddAppointment>
	{

		private readonly IAggregateRepository repository;

		public AddAppointmentCommandHandler(IAggregateRepository repository)
		{
			this.repository = repository;
		}

		public void Execute(AddAppointment command)
		{
			var aggregate = repository.GetById(command.AggregateId);

			aggregate.AddAppointment(command.UserId, 
									 command.PatientId, command.Description, 
									 command.Day, command.StartTime, command.EndTime, 
									 command.TherapyPlaceId, command.Room);
		}
	}
}
