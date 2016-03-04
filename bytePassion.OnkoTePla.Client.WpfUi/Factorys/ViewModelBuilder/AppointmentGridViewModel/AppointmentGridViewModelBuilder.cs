using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Contracts.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentGridViewModel
{
	internal class AppointmentGridViewModelBuilder : IAppointmentGridViewModelBuilder 
	{				
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly IClientReadModelRepository readModelRepository;		
		private readonly IViewModelCommunication viewModelCommunication;				
		private readonly ISharedStateReadOnly<Size> gridSizeVariable;
		private readonly ISharedStateReadOnly<Guid?> roomFilterVariable;				
		private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;
		private readonly ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder;

		public AppointmentGridViewModelBuilder(IClientMedicalPracticeRepository medicalPracticeRepository,
											   IClientReadModelRepository readModelRepository,
											   IViewModelCommunication viewModelCommunication,
											   ISharedStateReadOnly<Size> gridSizeVariable, 
											   ISharedStateReadOnly<Guid?> roomFilterVariable,
											   IAppointmentViewModelBuilder appointmentViewModelBuilder, 
											   ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder)
											   
		{						
			this.medicalPracticeRepository = medicalPracticeRepository;
			this.readModelRepository = readModelRepository;			
			this.viewModelCommunication = viewModelCommunication;						
			this.gridSizeVariable = gridSizeVariable;
			this.roomFilterVariable = roomFilterVariable;			
			this.appointmentViewModelBuilder = appointmentViewModelBuilder;
			this.therapyPlaceRowViewModelBuilder = therapyPlaceRowViewModelBuilder;
		}


		public void RequestBuild(Action<IAppointmentGridViewModel> viewModelAvailableCallback, AggregateIdentifier identifier, Action<string> errorCallback)
		{
			medicalPracticeRepository.RequestMedicalPractice(
				medicalPractice =>
				{
					if (medicalPractice.HoursOfOpening.IsOpen(identifier.Date))
					{
						therapyPlaceRowViewModelBuilder.RequestBuild(therapyPlaceRowViewModels =>
						{
							readModelRepository.RequestAppointmentsOfADayReadModel(readModel =>
							{
								viewModelAvailableCallback(new ViewModels.AppointmentGrid.AppointmentGridViewModel(identifier,
																											       medicalPractice,
																											       viewModelCommunication,
																											       gridSizeVariable,
																											       roomFilterVariable,
																											       appointmentViewModelBuilder,
																												   readModel,
																												   therapyPlaceRowViewModels,
																											       errorCallback));
							},
							identifier,
							errorCallback);							
						},
						identifier,
						medicalPractice.Rooms,
						errorCallback);						
					}
					else
					{
						viewModelAvailableCallback(new ClosedDayGridViewModel(identifier,
																			  medicalPractice,
																			  viewModelCommunication,
																			  gridSizeVariable));
					}
				},
				identifier.MedicalPracticeId,
				identifier.Date,
				errorCallback
			);
		}
	}
}
