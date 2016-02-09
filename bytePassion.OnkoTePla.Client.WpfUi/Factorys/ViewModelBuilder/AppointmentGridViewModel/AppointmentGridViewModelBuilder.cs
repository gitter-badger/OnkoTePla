using System;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.ReadModelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentGridViewModel
{
	internal class AppointmentGridViewModelBuilder : IAppointmentGridViewModelBuilder 
	{		
		private readonly ISession session;
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly IClientReadModelRepository readModelRepository;
		private readonly ICommandBus commandBus;
		private readonly IViewModelCommunication viewModelCommunication;				
		private readonly ISharedStateReadOnly<Size> gridSizeVariable;
		private readonly ISharedStateReadOnly<Guid?> roomFilterVariable;		
		private readonly ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable;		
		private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;
		private readonly ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder;

		public AppointmentGridViewModelBuilder(ISession session,
											   IClientMedicalPracticeRepository medicalPracticeRepository,
											   IClientReadModelRepository readModelRepository,
											   ICommandBus commandBus,
											   IViewModelCommunication viewModelCommunication, 											   
											   ISharedStateReadOnly<Size> gridSizeVariable, 
											   ISharedStateReadOnly<Guid?> roomFilterVariable, 											  
											   ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable, 											  
											   IAppointmentViewModelBuilder appointmentViewModelBuilder, 
											   ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder)
											   
		{			
			this.session = session;
			this.medicalPracticeRepository = medicalPracticeRepository;
			this.readModelRepository = readModelRepository;
			this.commandBus = commandBus;
			this.viewModelCommunication = viewModelCommunication;						
			this.gridSizeVariable = gridSizeVariable;
			this.roomFilterVariable = roomFilterVariable;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
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
						viewModelAvailableCallback(new ViewModels.AppointmentGrid.AppointmentGridViewModel(identifier,
																										   medicalPractice,
																										   session,
																										   commandBus,
																										   readModelRepository,
																										   viewModelCommunication,
																										   gridSizeVariable,
																										   roomFilterVariable,
																										   appointmentModificationsVariable,
																										   appointmentViewModelBuilder,
																										   therapyPlaceRowViewModelBuilder,
																										   errorCallback));
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
