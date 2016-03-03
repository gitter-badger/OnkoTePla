using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public class AddedAction : IUserAction
	{			
		public AddedAction(ICommandService commandService)
		{					
		}
		
		public void Undo()
		{
			
		}

		public void Redo()
		{
			throw new System.NotImplementedException();
		}

		public string GetUndoMsg()
		{
			throw new System.NotImplementedException();
		}

		public string GetRedoMsg ()
		{
			throw new System.NotImplementedException();
		}
	}
}