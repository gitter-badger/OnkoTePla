using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandHandler
{
	public class AddAppointmentCommandHandler : IDomainCommandHandler<AddAppointment>
	{

		//private readonly IAggregateRepository repository;

		public AddAppointmentCommandHandler() //IAggregateRepository repository)
		{
		//	this.repository = repository;
		}
		
		public void Process(AddAppointment command)
		{
//			var aggregate = repository.GetById(command.AggregateId);
//
//			aggregate.AddAppointment(command.UserId, 
//									 command.AggregateVersion, 
//									 command.ActionTag,
//									 command.CreateAppointmentData);
//
//			repository.Save(aggregate);
		}
	}
}
