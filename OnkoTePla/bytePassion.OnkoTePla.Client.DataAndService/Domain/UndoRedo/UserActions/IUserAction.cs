using System;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public interface IUserAction
	{		
		void Undo(Action<bool> operationResultCallback, Action<string> errorCallback);
		void Redo(Action<bool> operationResultCallback, Action<string> errorCallback);
		
		string GetUndoMsg();
		string GetRedoMsg();
	}
}
