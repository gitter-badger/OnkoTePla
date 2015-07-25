using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelectorViewModel.Helper
{
	public class RoomSelectorData
	{
		public static readonly RoomSelectorData Dummy = new RoomSelectorData(null, null);

		private readonly Room room;
		private readonly ICommand selectRoomCommand;

		public RoomSelectorData(Room room, ICommand selectRoomCommand)
		{
			this.room = room;
			this.selectRoomCommand = selectRoomCommand;
		}

		public Room Room
		{
			get { return room; }
		}

		public ICommand SelectRoomCommand
		{
			get { return selectRoomCommand; }
		}

		public bool IsDummy
		{
			get { return Equals(Dummy); }
		}
	}
}