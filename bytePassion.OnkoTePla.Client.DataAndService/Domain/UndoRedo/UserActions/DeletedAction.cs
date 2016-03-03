using System;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public class DeletedAction : IUserAction
	{
		public DeletedAction ()			
		{
		}
		
		public void Undo(Action<string> errorCallback)
		{
			throw new NotImplementedException();
		}

		public void Redo(Action<string> errorCallback)
		{
			throw new NotImplementedException();
		}

		public string GetUndoMsg ()
		{
			throw new System.NotImplementedException();
		}

		public string GetRedoMsg ()
		{
			throw new System.NotImplementedException();
		}
	}
}