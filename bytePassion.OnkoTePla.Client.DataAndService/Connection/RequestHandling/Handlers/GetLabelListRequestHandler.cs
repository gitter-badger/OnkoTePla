using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Config;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetLabelListRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<Label>> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;
		
		
		public GetLabelListRequestHandler (Action<IReadOnlyList<Label>> dataReceivedCallback, 
									      ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,
									      Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
		}
		
		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetLabelListRequest, GetLabelListResponse>(
				new GetLabelListRequest(connectionInfoVariable.Value.SessionId), 
				socket,				
				labelListResponse => dataReceivedCallback(labelListResponse.AvailableLabels)
			);
		}
	}
}
