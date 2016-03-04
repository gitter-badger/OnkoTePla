using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo
{
	public class UndoRedoService : IUndoRedoService
	{
		private class InitialListNode : IUserAction
		{
			public void Undo(Action<string> errorCallback) {}
			public void Redo(Action<string> errorCallback) {}
			public string GetUndoMsg() => null;			
			public string GetRedoMsg() => null;
		}

		private readonly int maximalSavedVersions;
		public event Action<bool> UndoPossibleChanged;
		public event Action<bool> RedoPossibleChanged;

		private readonly LinkedList<IUserAction> userActions;

		private LinkedListNode<IUserAction> currentActionPointer;
		private bool redoPossible;
		private bool undoPossible;

		public UndoRedoService(int maximalSavedVersions)
		{
			this.maximalSavedVersions = maximalSavedVersions;
			userActions = new LinkedList<IUserAction>();

			userActions.AddFirst(new InitialListNode());
			currentActionPointer = userActions.First;

			CheckIfUndoAndRedoIsPossible();
		}
				
		public bool RedoPossible
		{
			get { return redoPossible; }
			private set
			{
				if (value != RedoPossible)
				{
					redoPossible = value;
					RedoPossibleChanged?.Invoke(value);
				}				
			}
		}

		public bool UndoPossible
		{
			get { return undoPossible; }
			private set
			{
				if (value != UndoPossible)
				{
					undoPossible = value;
					UndoPossibleChanged?.Invoke(value);
				}
				
			}
		}

		public void Undo(Action<string> errorCallback)
		{
			if (!UndoPossible)
				errorCallback("undo impossible");

			currentActionPointer.Value.Undo(errorCallback);			// TODO: restart History on error
			currentActionPointer = currentActionPointer.Previous;

			CheckIfUndoAndRedoIsPossible();
		}

		public void Redo (Action<string> errorCallback)
		{
			if (!RedoPossible)
				errorCallback("redo impossible");

			currentActionPointer.Next.Value.Redo(errorCallback);    // TODO: restart History on error
			currentActionPointer = currentActionPointer.Next;

			CheckIfUndoAndRedoIsPossible();
		}

		public string GetCurrentUndoActionMsg()
		{
			if (!UndoPossible)
				throw new InvalidOperationException("undo impossible");

			return currentActionPointer.Value.GetUndoMsg();
		}
						
		public string GetCurrentRedoActionMsg()
		{
			if (!RedoPossible)
				throw new InvalidOperationException("redo impossible");

			return currentActionPointer.Next.Value.GetRedoMsg();
		}
		
		public void ReportUserAction(IUserAction newUserAction)
		{
			var newVersionNode = new LinkedListNode<IUserAction>(newUserAction);

			RemoveAllFromEndTo(currentActionPointer);
			userActions.AddLast(newVersionNode);
			currentActionPointer = newVersionNode;

			if (maximalSavedVersions >= 0 &&
			    userActions.Count == maximalSavedVersions + 1)
			{
				userActions.RemoveFirst();
			}

			CheckIfUndoAndRedoIsPossible();
		}

		private void RemoveAllFromEndTo (LinkedListNode<IUserAction> node)
		{
			while (userActions.Last != node)
			{
				userActions.RemoveLast();

				if (userActions.Count == 0)
					throw new InvalidOperationException();
			}
		}

		private void CheckIfUndoAndRedoIsPossible ()
		{
			UndoPossible = currentActionPointer.Previous != null;
			RedoPossible = currentActionPointer.Next     != null;
		}
	}
}
