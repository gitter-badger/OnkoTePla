using System;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public class DividedReplacedAction : IUserAction
	{
		public DividedReplacedAction()
		{
		}

		public void Undo(Action<string> errorCallback)
		{
			throw new System.NotImplementedException();
		}

		public void Redo(Action<string> errorCallback)
		{
			throw new System.NotImplementedException();
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