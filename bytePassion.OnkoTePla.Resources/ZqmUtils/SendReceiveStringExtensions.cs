using NetMQ;

namespace bytePassion.OnkoTePla.Resources.ZqmUtils
{
	public static class SendReceiveStringExtensions
	{
		public static void SendAString(this NetMQSocket socket, string message)
		{			
			var outMsg = new Msg();
			outMsg.InitPool(GlobalConstants.Encoding.GetByteCount(message));
			GlobalConstants.Encoding.GetBytes(message, 0, message.Length, outMsg.Data, 0);

			socket.Send(ref outMsg, more: false);

			outMsg.Close();
		}

		public static string ReceiveAString(this NetMQSocket socket)
		{
			var inMsg = new Msg();			
			inMsg.InitEmpty();

			socket.Receive(ref inMsg);

			var str = inMsg.Size > 0
				? GlobalConstants.Encoding.GetString(inMsg.Data, 0, inMsg.Size)
				: string.Empty;

			inMsg.Close();

			return str;
		}
	}	
}
