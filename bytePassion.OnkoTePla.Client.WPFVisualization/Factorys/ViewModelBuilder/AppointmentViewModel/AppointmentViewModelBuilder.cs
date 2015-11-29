using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.AppointmentViewModel
{
	public class AppointmentViewModelBuilder : IAppointmentViewModelBuilder 
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly IDataCenter dataCenter;
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;
		private readonly IGlobalState<Date> selectedDateVariable;
		private readonly IGlobalState<Size> gridSizeVariable;
		private readonly AdornerControl adornerControl;

		public AppointmentViewModelBuilder(IViewModelCommunication viewModelCommunication, 
										   IDataCenter dataCenter, 
										   IGlobalState<AppointmentModifications> appointmentModificationsVariable, 
										   IGlobalState<Date> selectedDateVariable, 
										   IGlobalState<Size> gridSizeVariable, AdornerControl adornerControl)
		{
			this.viewModelCommunication = viewModelCommunication;
			this.dataCenter = dataCenter;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.selectedDateVariable = selectedDateVariable;
			this.gridSizeVariable = gridSizeVariable;
			this.adornerControl = adornerControl;
		}

		public IAppointmentViewModel Build (Appointment appointment, AggregateIdentifier location)
		{
			var exactLocation = new TherapyPlaceRowIdentifier(location, appointment.TherapyPlace.Id);

			return new ViewModels.AppointmentView.AppointmentViewModel(appointment,
																	   viewModelCommunication,
																	   dataCenter,
																	   exactLocation,
																	   appointmentModificationsVariable,
																	   selectedDateVariable,
																	   gridSizeVariable,
																	   adornerControl);
		}
	}
}
