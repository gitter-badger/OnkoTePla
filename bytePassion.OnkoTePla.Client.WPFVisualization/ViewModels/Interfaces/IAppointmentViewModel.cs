using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IAppointmentViewModel : IViewModelBase
	{
		ICommand DeleteAppointment { get; }

		string PatientDisplayName { get; }		
		double CanvasPosition     { get; set; }
		double ViewElementLength  { get; set; }
	}
}
