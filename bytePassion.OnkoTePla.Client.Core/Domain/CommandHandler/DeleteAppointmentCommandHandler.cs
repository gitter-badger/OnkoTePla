using bytePassion.OnkoTePla.Client.Core.CommandSystem.Base;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;


namespace bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler
{
	public class DeleteAppointmentCommandHandler : IDomainCommandHandler<DeleteAppointment>
	{

		private readonly IAggregateRepository repository;

		public DeleteAppointmentCommandHandler(IAggregateRepository repository)
		{
			this.repository = repository;
		}

		public void Execute(DeleteAppointment command)
		{
			var aggregate = repository.GetById(command.AggregateId);

			aggregate.DeleteAppointment(command.UserId, command.AggregateVersion, command.PatientId, command.AppointmentId);

			repository.Save(aggregate);
		}
	}
}
