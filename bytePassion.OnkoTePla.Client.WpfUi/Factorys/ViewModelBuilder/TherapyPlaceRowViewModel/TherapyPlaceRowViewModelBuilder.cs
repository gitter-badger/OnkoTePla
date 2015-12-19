using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel
{
	internal class TherapyPlaceRowViewModelBuilder : ITherapyPlaceRowViewModelBuilder
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly IDataCenter dataCenter;
		private readonly AdornerControl adornerControl;
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;
		private readonly IGlobalState<Date> selectedDateVariable;
		private readonly IGlobalState<Guid> selectedMedicalPracticeIdVariable;

		public TherapyPlaceRowViewModelBuilder(IViewModelCommunication viewModelCommunication, 
											   IDataCenter dataCenter, AdornerControl adornerControl, 
											   IGlobalState<AppointmentModifications> appointmentModificationsVariable, 
											   IGlobalState<Date> selectedDateVariable, 
											   IGlobalState<Guid> selectedMedicalPracticeIdVariable)
		{
			this.viewModelCommunication = viewModelCommunication;
			this.dataCenter = dataCenter;
			this.adornerControl = adornerControl;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.selectedDateVariable = selectedDateVariable;
			this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;
		}

		public ITherapyPlaceRowViewModel Build(TherapyPlace therapyPlace, Room room, TherapyPlaceRowIdentifier location)
		{
			return new ViewModels.TherapyPlaceRowView.TherapyPlaceRowViewModel(viewModelCommunication,
																			   dataCenter,
																			   therapyPlace,
																			   room.DisplayedColor,
																			   location,
																			   adornerControl,
																			   appointmentModificationsVariable,
																			   selectedDateVariable,
																			   selectedMedicalPracticeIdVariable);
		}
	}
}
