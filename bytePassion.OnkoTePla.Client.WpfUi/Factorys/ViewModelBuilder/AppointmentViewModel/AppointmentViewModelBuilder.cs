using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.AppointmentModification;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;



namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel
{
	internal class AppointmentViewModelBuilder : IAppointmentViewModelBuilder 
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ICommandService commandService;
		private readonly ISharedState<ViewModels.AppointmentView.Helper.AppointmentModifications> appointmentModificationsVariable;
		private readonly ISharedState<Date> selectedDateVariable;		
		private readonly AdornerControl adornerControl;
	    private readonly IAppointmentModificationsBuilder appointmentModificationsBuilder;

	    public AppointmentViewModelBuilder(IViewModelCommunication viewModelCommunication, 	
										   ICommandService commandService,									   
										   ISharedState<ViewModels.AppointmentView.Helper.AppointmentModifications> appointmentModificationsVariable, 
										   ISharedState<Date> selectedDateVariable, 
										   AdornerControl adornerControl,
                                           IAppointmentModificationsBuilder appointmentModificationsBuilder)
		{
			this.viewModelCommunication = viewModelCommunication;
		    this.commandService = commandService;
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.selectedDateVariable = selectedDateVariable;			
			this.adornerControl = adornerControl;
	        this.appointmentModificationsBuilder = appointmentModificationsBuilder;
		}

		public IAppointmentViewModel Build (Appointment appointment, AggregateIdentifier location, Action<string> errorCallback)
		{			
            var editDescriptionWindowBuilder = new EditDescriptionWindowBuilder(appointment, 																				
																				appointmentModificationsVariable,
																				errorCallback);
			
			return new ViewModels.AppointmentView.AppointmentViewModel(appointment,
																	   commandService,
																	   viewModelCommunication,
																	   new TherapyPlaceRowIdentifier(location, appointment.TherapyPlace.Id),
																	   appointmentModificationsVariable,
																	   selectedDateVariable,
																	   appointmentModificationsBuilder,
																	   editDescriptionWindowBuilder,
																	   adornerControl,
																	   errorCallback);
		}
	}
}
