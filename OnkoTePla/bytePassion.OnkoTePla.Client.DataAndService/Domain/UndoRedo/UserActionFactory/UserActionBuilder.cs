using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActionFactory
{
	public class UserActionBuilder : IUserActionBuilder
	{
		private readonly ICommandService commandService;

		public UserActionBuilder(ICommandService commandService)
		{
			this.commandService = commandService;
		}

		public AddedAction BuildAddedAction(AddAppointment command, Patient patient)
		{
			return new AddedAction(commandService, command, patient);
		}

		public DeletedAction BuildDeletedAction(DeleteAppointment command, Patient patient)
		{
			return new DeletedAction(commandService, command, patient);
		}

		public ReplacedAction BuildReplacedAction(ReplaceAppointment command, Patient patient, TherapyPlace originalTherapyPlace, TherapyPlace newTherapyPlace)
		{
			return new ReplacedAction(commandService, command, patient, originalTherapyPlace, newTherapyPlace);
		}
	}
}
