using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid
{

	public interface ITimeGridViewModel
	{
		ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; }
		ObservableCollection<TimeSlotLine>  TimeSlotLines  { get; }
	}
}