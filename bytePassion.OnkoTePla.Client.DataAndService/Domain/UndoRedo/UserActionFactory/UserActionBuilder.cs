using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions;
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
	}
}
