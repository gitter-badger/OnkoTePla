using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActionFactory
{
	public interface IUserActionBuilder
	{
		AddedAction   BuildAddedAction  (AddAppointment    command, Patient patient);
		DeletedAction BuildDeletedAction(DeleteAppointment command, Patient patient);
	}
}