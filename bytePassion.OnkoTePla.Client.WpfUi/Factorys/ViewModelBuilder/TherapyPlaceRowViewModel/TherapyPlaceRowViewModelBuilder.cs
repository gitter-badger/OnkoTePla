using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Domain;
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
		
		public void RequestBuild(Action<IEnumerable<ITherapyPlaceRowViewModel>> viewModelsAvailable, 
								 AggregateIdentifier identifier, IEnumerable<Room> rooms, 
								 Action<string> errorCallback)
		{
			medicalPracticeRepository.RequestMedicalPractice(
				practice =>
				{

					var viewModels = new List<ITherapyPlaceRowViewModel>();

					foreach (var room in rooms)
					{
						foreach (var therapyPlace in room.TherapyPlaces)
						{
							var location = new TherapyPlaceRowIdentifier(identifier, therapyPlace.Id);

							var hoursOfOpeing = practice.HoursOfOpening;

							var newViewModel = new ViewModels.TherapyPlaceRowView.TherapyPlaceRowViewModel(viewModelCommunication,
																										   therapyPlace,
																										   room.DisplayedColor,
																										   location,
																										   adornerControl,
																										   hoursOfOpeing.GetOpeningTime(location.PlaceAndDate.Date),
																										   hoursOfOpeing.GetClosingTime(location.PlaceAndDate.Date),
																										   appointmentModificationsVariable,
																										   appointmentGridSizeVariable.Value.Width);
							viewModels.Add(newViewModel);
						}
					}

					viewModelsAvailable(viewModels);
				},
				identifier.MedicalPracticeId,
				identifier.PracticeVersion,
				errorCallback	
			);
		}
	}
}
