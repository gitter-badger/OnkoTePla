using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IAppointmentGridViewModel : IViewModelBase
	{
		ICommand LoadReadModel { get; }

		ObservableCollection<TimeSlotLabel>             TimeSlotLabels   { get; }
		ObservableCollection<TimeSlotLine>              TimeSlotLines    { get; }
		ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get; } 

		double CurrentGridWidth  { set; get; }
		double CurrentGridHeight { set; get; }

		IAppointmentViewModel   EditingObject { get; set; }
		AppointmentGridViewMode OperatingMode { get; }
	}
}
