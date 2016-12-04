namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class TryToAddNewEventsResponse : NetworkMessageBase
	{
		public TryToAddNewEventsResponse(bool addingWasSuccessful)
			: base(NetworkMessageType.TryToAddNewEventsResponse)
		{
			AddingWasSuccessful = addingWasSuccessful;
		}

		public bool AddingWasSuccessful { get; }

		public override string AsString()
		{
			return AddingWasSuccessful.ToString();
		}

		public static TryToAddNewEventsResponse Parse(string s)
		{
			return new TryToAddNewEventsResponse(bool.Parse(s));
		}
	}
}