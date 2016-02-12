using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
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
		private readonly ISharedState<AppointmentModifications> appointmentModificationsVariable;		

		public TherapyPlaceRowViewModelBuilder(IViewModelCommunication viewModelCommunication,
											   IClientMedicalPracticeRepository medicalPracticeRepository, 
											   AdornerControl adornerControl, 
											   ISharedState<AppointmentModifications> appointmentModificationsVariable)
		{
			this.viewModelCommunication = viewModelCommunication;
			this.medicalPracticeRepository = medicalPracticeRepository;

			this.adornerControl = adornerControl;
			this.appointmentModificationsVariable = appointmentModificationsVariable;		
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
																					appointmentModificationsVariable)
					);					
				},
				location.PlaceAndDate.MedicalPracticeId,
				location.PlaceAndDate.PracticeVersion,
				errorCallback					
			);
		}
	}
}
