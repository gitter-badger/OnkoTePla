namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public class DeletedAction : IUserAction
	{
		public DeletedAction ()			
		{
		}

		public void Undo()
		{
			throw new System.NotImplementedException();
		}

		public void Redo()
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