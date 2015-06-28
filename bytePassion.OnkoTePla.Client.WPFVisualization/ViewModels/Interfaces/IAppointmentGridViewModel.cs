using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IAppointmentGridViewModel : IViewModelBase
	{
		ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; }
		ObservableCollection<TimeSlotLine>  TimeSlotLines {  get; } 

		double CurrentGridWidth { set; get; }
		double CurrentGridHeight { set; get; }
	}
}
