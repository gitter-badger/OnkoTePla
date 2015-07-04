using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IAppointmentViewModel : IViewModelBase
	{
		string PatientDisplayName { get; }
		double FrameworkElementWidth { get; }

		Time StartTime { get; set; }
		Time EndTime { get; set; }
	}
}
