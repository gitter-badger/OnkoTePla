using System;
using System.Text;
using NetMQ;

namespace bytePassion.Lib.ZmqUtils
{
	public static class SendReceiveStringExtensions
	{
		public static readonly TimeSpan InfiniteTimeout = TimeSpan.FromMilliseconds(-1.0);

		private static readonly Encoding Encoding = new UTF8Encoding();

		public static bool SendAString(this NetMQSocket socket, string message, TimeSpan timeout)
		{			 
			var outMsg = new Msg();
			outMsg.InitPool(Encoding.GetByteCount(message));
			Encoding.GetBytes(message, 0, message.Length, outMsg.Data, 0);
			
			var sendSuccessful = socket.TrySend(ref outMsg, timeout, false);			

			outMsg.Close();

			return sendSuccessful;
		}

		public static string ReceiveAString(this NetMQSocket socket, TimeSpan timeout)
		{
			var inMsg = new Msg();			
			inMsg.InitEmpty();
			
			socket.TryReceive(ref inMsg, timeout);

			var str = inMsg.Size > 0
				? Encoding.GetString(inMsg.Data, 0, inMsg.Size)
				: string.Empty;

			inMsg.Close();

			return str;
		}
	}	
}
