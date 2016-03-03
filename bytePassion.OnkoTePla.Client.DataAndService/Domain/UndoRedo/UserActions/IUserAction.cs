namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public interface IUserAction
	{		
		void Undo();
		void Redo();

		string GetUndoMsg();
		string GetRedoMsg();
	}
}
