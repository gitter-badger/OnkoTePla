using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandHandler
{
	public class DeleteAppointmentCommandHandler : IDomainCommandHandler<DeleteAppointment>
	{

		//private readonly IAggregateRepository repository;

		public DeleteAppointmentCommandHandler()// IAggregateRepository repository)
		{
			//this.repository = repository;
		}

		public void Process(DeleteAppointment command)
		{
//			var aggregate = repository.GetById(command.AggregateId);
//
//			aggregate.DeleteAppointment(command.UserId, 
//										command.AggregateVersion, 
//										command.PatientId, 
//										command.ActionTag, 
//										command.AppointmentToRemoveId);
//
//			repository.Save(aggregate);
		}		
	}
}
