using System;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.AppointmentGridViewModel
{
	public class AppointmentGridViewModelBuilder : IAppointmentGridViewModelBuilder 
	{
		private readonly IDataCenter dataCenter;
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ICommandBus commandBus;
		private readonly AdornerControl adornerControl;
		private readonly IGlobalState<Size> gridSizeVariable;
		private readonly IGlobalState<Guid?> roomFilterVariable;
		private readonly IGlobalState<Date> selectedDateVariable;
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;
		private readonly IGlobalState<Guid> selectedMedicalPracticeIdVariable;

		public AppointmentGridViewModelBuilder(IDataCenter dataCenter, 
											   IViewModelCommunication viewModelCommunication, 
											   ICommandBus commandBus, 
											   AdornerControl adornerControl, 
											   IGlobalState<Size> gridSizeVariable, 
											   IGlobalState<Guid?> roomFilterVariable, 
											   IGlobalState<Date> selectedDateVariable, 
											   IGlobalState<AppointmentModifications> appointmentModificationsVariable, 
											   IGlobalState<Guid> selectedMedicalPracticeIdVariable)
		{
			this.dataCenter = dataCenter;
			this.viewModelCommunication = viewModelCommunication;
			this.commandBus = commandBus;
			this.adornerControl = adornerControl;
			this.gridSizeVariable = gridSizeVariable;
			this.roomFilterVariable = roomFilterVariable;
			this.selectedDateVariable = selectedDateVariable;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;
		}

		public IAppointmentGridViewModel Build(AggregateIdentifier identifier)
		{
			var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(identifier.Date, identifier.MedicalPracticeId);

			IAppointmentGridViewModel gridViewModel;

			if (medicalPractice.HoursOfOpening.IsOpen(identifier.Date))
				gridViewModel = new ViewModels.AppointmentGrid.AppointmentGridViewModel(identifier,
															 dataCenter,
															 commandBus,
															 viewModelCommunication,
															 gridSizeVariable,
															 roomFilterVariable,
															 selectedDateVariable,
															 appointmentModificationsVariable,
															 selectedMedicalPracticeIdVariable,
															 adornerControl);
			else
				gridViewModel = new ClosedDayGridViewModel(identifier,
														   dataCenter,
														   viewModelCommunication,
														   gridSizeVariable);

			return gridViewModel;
		}
	}
}
