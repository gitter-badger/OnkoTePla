using System.Collections.Generic;


namespace xIT.OnkoTePla.Contracts.Infrastructure
{
	public sealed class Room
	{
		private readonly uint roomID;
		private readonly IReadOnlyList<TherapyPlace> availableTherapyPlaces;

		public Room(uint roomId, IReadOnlyList<TherapyPlace> availableTherapyPlaces)
		{
			roomID = roomId;
			this.availableTherapyPlaces = availableTherapyPlaces;
		}

		public uint RoomID
		{
			get { return roomID; }
		}

		public IReadOnlyList<TherapyPlace> AvailableTherapyPlaces
		{
			get { return availableTherapyPlaces; }
		} 
	}
}
