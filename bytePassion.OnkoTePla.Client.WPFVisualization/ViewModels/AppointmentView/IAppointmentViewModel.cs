using System;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public interface IAppointmentViewModel : IViewModel,
											 IViewModelCollectionItem<Guid>,
											 IDisposable,
                                             IViewModelMessageHandler<Dispose>,
											 IViewModelMessageHandler<NewSizeAvailable>                                             
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



		ICommand SaveCanvasLeftPosition  { get; }
		double   CanvasLeftPositionDelta { set; }
	}
}
