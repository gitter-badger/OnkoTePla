using bytePassion.OnkoTePla.Contracts.NetworkMessages;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	public abstract class RequestObject
	{
		protected RequestObject(NetworkMessageType requestType)
		{
			RequestType = requestType;
		}

		public NetworkMessageType RequestType { get; }
	}
}
