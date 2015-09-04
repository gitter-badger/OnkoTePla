using System;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public interface IAppointmentViewModel : IViewModel, IDisposable
	{
		ICommand DeleteAppointment { get; }
		ICommand SwitchToEditMode  { get; }
		
		double CanvasLeftPosition { get; set; }
		double ViewElementLength  { get; set; }

		string PatientDisplayName { get; }
		string TimeSpan           { get; }
		string AppointmentDate    { get; }
		string Description        { get; }
		string Room               { get; }

		OperatingMode OperatingMode { get; }
	}
}
