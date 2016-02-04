using System;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentGridViewModel
{
	internal class AppointmentGridViewModelBuilder : IAppointmentGridViewModelBuilder 
	{
		private readonly IDataCenter dataCenter;
		private readonly ISession session;
		private readonly IViewModelCommunication viewModelCommunication;				
		private readonly ISharedStateReadOnly<Size> gridSizeVariable;
		private readonly ISharedStateReadOnly<Guid?> roomFilterVariable;		
		private readonly ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable;		
		private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;
		private readonly ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder;

		public AppointmentGridViewModelBuilder(IDataCenter dataCenter, 
											   ISession session,
											   IViewModelCommunication viewModelCommunication, 											   
											   ISharedStateReadOnly<Size> gridSizeVariable, 
											   ISharedStateReadOnly<Guid?> roomFilterVariable, 											  
											   ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable, 											  
											   IAppointmentViewModelBuilder appointmentViewModelBuilder, 
											   ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder)
											   
		{
			this.dataCenter = dataCenter;
			this.session = session;
			this.viewModelCommunication = viewModelCommunication;						
			this.gridSizeVariable = gridSizeVariable;
			this.roomFilterVariable = roomFilterVariable;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.appointmentViewModelBuilder = appointmentViewModelBuilder;
			this.therapyPlaceRowViewModelBuilder = therapyPlaceRowViewModelBuilder;
		}

		public IAppointmentGridViewModel Build(AggregateIdentifier identifier)
		{
			var medicalPractice = dataCenter.GetMedicalPracticeByIdAndDate(identifier.MedicalPracticeId, identifier.Date);

			IAppointmentGridViewModel gridViewModel;

			if (medicalPractice.HoursOfOpening.IsOpen(identifier.Date))
			{
				gridViewModel = new ViewModels.AppointmentGrid.AppointmentGridViewModel(identifier,
																						dataCenter,
																						session,																						
																						viewModelCommunication,
																						gridSizeVariable,
																						roomFilterVariable,																						
																						appointmentModificationsVariable,																						
																						appointmentViewModelBuilder,
																						therapyPlaceRowViewModelBuilder);
			}
			else
			{
				gridViewModel = new ClosedDayGridViewModel(identifier,
														   dataCenter,
														   viewModelCommunication,
														   gridSizeVariable);
			}
            				
			return gridViewModel;
		}
	}
}
