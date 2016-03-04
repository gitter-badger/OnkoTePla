using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
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
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly AdornerControl adornerControl;
		private readonly ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable;
		private readonly ISharedStateReadOnly<Size> appointmentGridSizeVariable;

		public TherapyPlaceRowViewModelBuilder(IViewModelCommunication viewModelCommunication,
											   IClientMedicalPracticeRepository medicalPracticeRepository, 
											   AdornerControl adornerControl, 
											   ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable,
											   ISharedStateReadOnly<Size> appointmentGridSizeVariable)
		{
			this.viewModelCommunication = viewModelCommunication;
			this.medicalPracticeRepository = medicalPracticeRepository;

			this.adornerControl = adornerControl;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.appointmentGridSizeVariable = appointmentGridSizeVariable;
		}		
		
		public void RequestBuild(Action<ITherapyPlaceRowViewModel> viewModelAvailable, 
								 TherapyPlace therapyPlace, Room room, TherapyPlaceRowIdentifier location,
								 Action<string> errorCallback)
		{
			medicalPracticeRepository.RequestMedicalPractice(
				practice =>
				{
					var hoursOfOpeing = practice.HoursOfOpening;

					viewModelAvailable(
						new ViewModels.TherapyPlaceRowView.TherapyPlaceRowViewModel(viewModelCommunication,
																					therapyPlace,
																					room.DisplayedColor,
																					location,
																					adornerControl,
																					hoursOfOpeing.GetOpeningTime(location.PlaceAndDate.Date),
																					hoursOfOpeing.GetClosingTime(location.PlaceAndDate.Date),
																					appointmentModificationsVariable,
																					appointmentGridSizeVariable.Value.Width)
					);					
				},
				location.PlaceAndDate.MedicalPracticeId,
				location.PlaceAndDate.PracticeVersion,
				errorCallback					
			);
		}
	}
}
