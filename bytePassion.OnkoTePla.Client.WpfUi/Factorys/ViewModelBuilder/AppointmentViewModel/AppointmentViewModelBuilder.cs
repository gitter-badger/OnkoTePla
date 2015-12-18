using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.AppointmentModification;
using bytePassion.OnkoTePla.Client.WpfUi.ServiceModules;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel
{
    internal class AppointmentViewModelBuilder : IAppointmentViewModelBuilder 
	{
		private readonly IViewModelCommunication viewModelCommunication;		
		private readonly IGlobalState<ViewModels.AppointmentView.Helper.AppointmentModifications> appointmentModificationsVariable;
		private readonly IGlobalState<Date> selectedDateVariable;		
		private readonly AdornerControl adornerControl;
	    private readonly IAppointmentModificationsBuilder appointmentModificationsBuilder;

	    public AppointmentViewModelBuilder(IViewModelCommunication viewModelCommunication, 										   
										   IGlobalState<ViewModels.AppointmentView.Helper.AppointmentModifications> appointmentModificationsVariable, 
										   IGlobalState<Date> selectedDateVariable, 
										   AdornerControl adornerControl,
                                           IAppointmentModificationsBuilder appointmentModificationsBuilder)
		{
			this.viewModelCommunication = viewModelCommunication;			
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.selectedDateVariable = selectedDateVariable;			
			this.adornerControl = adornerControl;
	        this.appointmentModificationsBuilder = appointmentModificationsBuilder;
		}

		public IAppointmentViewModel Build (Appointment appointment, AggregateIdentifier location)
		{
			var exactLocation = new TherapyPlaceRowIdentifier(location, appointment.TherapyPlace.Id);

			return new ViewModels.AppointmentView.AppointmentViewModel(appointment,
																	   viewModelCommunication,																	  
																	   exactLocation,
																	   appointmentModificationsVariable,
																	   selectedDateVariable,
																	   appointmentModificationsBuilder,
																	   adornerControl);
		}
	}
}
