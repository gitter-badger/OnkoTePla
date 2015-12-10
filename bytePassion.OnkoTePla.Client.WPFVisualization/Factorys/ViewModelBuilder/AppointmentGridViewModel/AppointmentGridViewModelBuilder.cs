using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using System;
using System.Windows;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.AppointmentGridViewModel
{
	public class AppointmentGridViewModelBuilder : IAppointmentGridViewModelBuilder 
	{
		private readonly IDataCenter dataCenter;
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ICommandBus commandBus;		
		private readonly IGlobalState<Size> gridSizeVariable;
		private readonly IGlobalState<Guid?> roomFilterVariable;		
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;		
		private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;
		private readonly ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder;

		public AppointmentGridViewModelBuilder(IDataCenter dataCenter, 
											   IViewModelCommunication viewModelCommunication, 
											   ICommandBus commandBus, 											  
											   IGlobalState<Size> gridSizeVariable, 
											   IGlobalState<Guid?> roomFilterVariable, 											  
											   IGlobalState<AppointmentModifications> appointmentModificationsVariable, 											  
											   IAppointmentViewModelBuilder appointmentViewModelBuilder, 
											   ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder)
											   
		{
			this.dataCenter = dataCenter;
			this.viewModelCommunication = viewModelCommunication;
			this.commandBus = commandBus;			
			this.gridSizeVariable = gridSizeVariable;
			this.roomFilterVariable = roomFilterVariable;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.appointmentViewModelBuilder = appointmentViewModelBuilder;
			this.therapyPlaceRowViewModelBuilder = therapyPlaceRowViewModelBuilder;
		}

		public IAppointmentGridViewModel Build(AggregateIdentifier identifier)
		{
			var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(identifier.Date, identifier.MedicalPracticeId);

			IAppointmentGridViewModel gridViewModel;

			if (medicalPractice.HoursOfOpening.IsOpen(identifier.Date))
			{
				gridViewModel = new ViewModels.AppointmentGrid.AppointmentGridViewModel(identifier,
																						dataCenter,
																						commandBus,
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
