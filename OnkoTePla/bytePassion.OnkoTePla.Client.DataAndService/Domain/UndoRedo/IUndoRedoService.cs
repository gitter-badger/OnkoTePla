using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo
{
	public interface IUndoRedoService
	{
		event Action<bool> UndoPossibleChanged;
		event Action<bool> RedoPossibleChanged;

		bool UndoPossible { get; }
		void Undo(Action<bool> operationResultCallback, Action<string> errorCallback);
		string GetCurrentUndoActionMsg();
		
		bool RedoPossible { get; }
		void Redo(Action<bool> operationResultCallback, Action<string> errorCallback);
		string GetCurrentRedoActionMsg();
		
		void ReportUserAction (IUserAction newUserAction);
		void ResetHistory();
	}
}