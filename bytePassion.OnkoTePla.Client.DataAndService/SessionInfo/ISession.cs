using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.DataAndService.SessionInfo
{
	public interface ISession
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                 applicationState                                  ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		event Action<ApplicationState> ApplicationStateChanged;		
		ApplicationState CurrentApplicationState { get; }



		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                       user                                        ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		event Action<IReadOnlyList<User>> UserListAvailable;

		User LoggedInUser { get; }
		IReadOnlyList<User> AvailableUsers { get; }

		void TryLogin(User user, string password);
		void Logout();



		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                 serverConnection                                  ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		void TryConnect     (Address serverAddress, Address clientAddress);
		void TryDebugConnect(Address serverAddress, Address clientAddress);
		void TryDisconnect();



		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                    undo / redo                                    ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		event Action<bool> UndoPossibleChanged;
		event Action<bool> RedoPossibleChanged;

		bool UndoPossible();
		void Undo();
		string GetUndoActionMessage();		

		bool RedoPossible();
		void Redo(); 
		string GetRedoActionMessage();		
	}
}
