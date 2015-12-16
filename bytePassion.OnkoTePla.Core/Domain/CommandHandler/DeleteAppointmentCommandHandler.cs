using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain.Commands;
using bytePassion.OnkoTePla.Core.Repositories.Aggregate;


namespace bytePassion.OnkoTePla.Core.Domain.CommandHandler
{
    public class DeleteAppointmentCommandHandler : IDomainCommandHandler<DeleteAppointment>
	{

		private readonly IAggregateRepository repository;

		public DeleteAppointmentCommandHandler(IAggregateRepository repository)
		{
			this.repository = repository;
		}

		public void Process(DeleteAppointment command)
		{
			var aggregate = repository.GetById(command.AggregateId);

			aggregate.DeleteAppointment(command.UserId, 
										command.AggregateVersion, 
										command.PatientId, 
										command.ActionTag, 
										command.AppointmentToRemoveId);

			repository.Save(aggregate);
		}		
	}
}
