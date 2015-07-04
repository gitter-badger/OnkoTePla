using System.Windows.Media;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface ITherapyPlaceRowViewModel
	{
		double TimeSlotWidth { set; }
		string TherapyPlaceName { get; }
		Color RoomColor { get; }
	}
}