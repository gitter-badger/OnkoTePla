using System.Windows.Media;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class TherapyPlaceRowViewModelSampleData : ITherapyPlaceRowViewModel
	{
		public TherapyPlaceRowViewModelSampleData()
		{
			TherapyPlaceName = "place 1";
			RoomColor = Colors.LightBlue;
		}

		public double TimeSlotWidth { set {} }

		public string TherapyPlaceName { get; private set; }
		public Color RoomColor { get; private set; }
	}
}