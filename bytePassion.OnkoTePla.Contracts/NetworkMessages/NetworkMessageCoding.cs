using System;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.RequestsAndResponses;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages
{
	public static class NetworkMessageCoding
	{
		public static string Decode(NetworkMessageBase msg)
		{
			return msg.Type + "|" +  msg.AsString();
		}
		
		public static NetworkMessageBase Encode(string messageString)
		{
			var type = (NetworkMessageType)Enum.Parse(typeof(NetworkMessageType), GetTypeFromMsg(messageString));
			var msg = GetMsgContent(messageString);

			switch (type)
			{
				case NetworkMessageType.GetUserListRequest:  return UserListRequest.Parse(msg);
				case NetworkMessageType.GetUserListResponse: return UserListResponse.Parse(msg);
				case NetworkMessageType.ErrorResponse:       return ErrorResponse.Parse(msg);

				default:
					throw new ArgumentException();
			}
		}

		private static string GetTypeFromMsg(string messageString)
		{
			var index = messageString.IndexOf("|", StringComparison.Ordinal);

			if (index == -1)
				throw new ArgumentException("inner error @ message decoding");

			return messageString.Substring(0, index);
		}

		private static string GetMsgContent(string messageString)
		{
			var index = messageString.IndexOf("|", StringComparison.Ordinal);

			if (index == -1)
				throw new ArgumentException("inner error @ message decoding");

			return messageString.Substring(index + 1, messageString.Length - index - 1);
		}
	}
}
