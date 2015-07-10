using System;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IAppointmentViewModel : IViewModelBase, IDisposable
	{
		ICommand DeleteAppointment { get; }
		ICommand SwitchToEditMode  { get; }

		string PatientDisplayName { get; }		
		double CanvasPosition     { get; set; }
		double ViewElementLength  { get; set; }

		Guid AppointmentId  { get; }
		Guid TherapyPlaceId { get; }

		OperatingMode OperatingMode { get; }
	}
}
