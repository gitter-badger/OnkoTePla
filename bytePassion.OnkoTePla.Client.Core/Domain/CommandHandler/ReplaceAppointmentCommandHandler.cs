using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;


namespace bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler
{
	public class ReplaceAppointmentCommandHandler : IDomainCommandHandler<ReplaceAppointment>
	{
		private readonly IAggregateRepository repository;

		public ReplaceAppointmentCommandHandler (IAggregateRepository repository)
		{
			this.repository = repository;
		}
		
		public void Process(ReplaceAppointment command)
		{
			var aggregate = repository.GetById(command.AggregateId);

			aggregate.ReplaceAppointemnt(command.UserId, 
										 command.AggregateVersion, 
										 command.PatientId,
										 command.ReplaceAppointmentData);

			repository.Save(aggregate);
		}
	}
}
