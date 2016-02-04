﻿using System;
using System.Text;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Communication.SendReceive
{
	public static class SendReceiveExtensions
	{
		public static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1.0);

		private static readonly Encoding Encoding = new UTF8Encoding();

		public static bool SendNetworkMsg(this NetMQSocket socket, NetworkMessageBase message, 
										  uint timeoutMilliSeconds = GlobalConstants.StandardSendingTimeout)
		{
			var encodedMessage = NetworkMessageCoding.Encode(message);

			var outMsg = new Msg();
			outMsg.InitPool(Encoding.GetByteCount(encodedMessage));
			Encoding.GetBytes(encodedMessage, 0, encodedMessage.Length, outMsg.Data, 0);
			
			var sendSuccessful = socket.TrySend(ref outMsg, TimeSpan.FromMilliseconds(timeoutMilliSeconds), false);			

			outMsg.Close();

			return sendSuccessful;
		}

		public static NetworkMessageBase ReceiveNetworkMsg(this NetMQSocket socket, TimeSpan timeout)
		{
			var inMsg = new Msg();			
			inMsg.InitEmpty();
			
			socket.TryReceive(ref inMsg, timeout);

			var str = inMsg.Size > 0
				? Encoding.GetString(inMsg.Data, 0, inMsg.Size)
				: string.Empty;

			inMsg.Close();

			return NetworkMessageCoding.Decode(str);				
		}
	}	
}