using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain.Commands;
using bytePassion.OnkoTePla.Core.Repositories.Aggregate;


namespace bytePassion.OnkoTePla.Core.Domain.CommandHandler
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

			aggregate.AddAppointment(command.UserId, 
									 command.AggregateVersion, 
									 command.ActionTag,
									 command.CreateAppointmentData);

			repository.Save(aggregate);
		}
	}
}
