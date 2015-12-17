using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using System;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView
{
    internal interface IAppointmentViewModel : IViewModel,
											   IViewModelCommunicationDeliverer,
											   IViewModelCollectionItem<Guid>,											
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

		AppointmentModifications CurrentAppointmentModifications { get; }
		AdornerControl AdornerControl { get; }

		OperatingMode OperatingMode       { get; }
		bool          ShowDisabledOverlay { get; }	
	}
}
