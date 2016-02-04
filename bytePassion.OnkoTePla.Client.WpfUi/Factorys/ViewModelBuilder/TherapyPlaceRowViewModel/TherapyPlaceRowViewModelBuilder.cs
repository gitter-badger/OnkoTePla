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
		private readonly ISharedState<AppointmentModifications> appointmentModificationsVariable;
		private readonly ISharedState<Date> selectedDateVariable;
		private readonly ISharedState<Guid> selectedMedicalPracticeIdVariable;

		public TherapyPlaceRowViewModelBuilder(IViewModelCommunication viewModelCommunication, 
											   IDataCenter dataCenter, AdornerControl adornerControl, 
											   ISharedState<AppointmentModifications> appointmentModificationsVariable, 
											   ISharedState<Date> selectedDateVariable, 
											   ISharedState<Guid> selectedMedicalPracticeIdVariable)
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
