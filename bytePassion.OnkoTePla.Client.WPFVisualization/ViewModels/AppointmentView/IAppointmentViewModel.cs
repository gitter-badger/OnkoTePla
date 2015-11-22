using System;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public interface IAppointmentViewModel : IViewModel,
											 IViewModelCommunicationDeliverer,
											 IViewModelCollectionItem<Guid>,
											 IDisposable,
                                             IViewModelMessageHandler<Dispose>,											 
											 IViewModelMessageHandler<RestoreOriginalValues>,
											 IViewModelMessageHandler<ShowDisabledOverlay>,
											 IViewModelMessageHandler<HideDisabledOverlay>,
											 IViewModelMessageHandler<SwitchToEditMode>
	{
		ICommand DeleteAppointment { get; }
		ICommand SwitchToEditMode  { get; }		
		
		Time BeginTime { get; }
		Time EndTime   { get; }
	
		string PatientDisplayName { get; }

		string TimeSpan           { get; }		//
		string AppointmentDate    { get; }		//	Information for
		string Description        { get; }		//  the Tool-Tip
		string Room               { get; }		//

		OperatingMode OperatingMode       { get; }
		bool          ShowDisabledOverlay { get; }	
	}
}
