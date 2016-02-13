using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class TryToAddNewEventsRequestHander : RequestHandlerBase
	{
		private readonly Action<bool> resultCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;
		private readonly IReadOnlyList<DomainEvent> newEvents;

		public TryToAddNewEventsRequestHander(Action<bool> resultCallback, 
											  ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,
											  IReadOnlyList<DomainEvent> newEvents, 
											  Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.resultCallback = resultCallback;
			this.connectionInfoVariable = connectionInfoVariable;
			this.newEvents = newEvents;
		}

		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<TryToAddNewEventsRequest, TryToAddNewEventsResponse>(
				new TryToAddNewEventsRequest(connectionInfoVariable.Value.SessionId, 
											 connectionInfoVariable.Value.LoggedInUser.Id, 
											 newEvents),
				socket,
				response => resultCallback(response.AddingWasSuccessful) 	
			);
		}
	}
}