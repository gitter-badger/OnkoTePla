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
		 
		ClientUserData LoggedInUser { get; }
		void RequestUserList(Action<IReadOnlyList<ClientUserData>> dataReceivedCallback,
							 Action<string> errorCallback); 
		
		void TryLogin(ClientUserData user, string password);
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
