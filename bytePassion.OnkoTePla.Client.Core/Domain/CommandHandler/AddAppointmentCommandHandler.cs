using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
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
		
		public void Process(AddAppointment command)
		{
			var aggregate = repository.GetById(command.AggregateId);

			aggregate.AddAppointment(command.UserId, command.AggregateVersion,
									 command.CreateAppointmentData);

			repository.Save(aggregate);
		}
	}
}
