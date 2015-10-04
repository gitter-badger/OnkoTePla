using System;
using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.Views;
using bytePassion.OnkoTePla.Contracts.Patients;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.WindowBuilder
{
	public class AddAppointmentDialogWindowBuilder : IWindowBuilder<AddAppointmentDialog>
	{
		private readonly IDataCenter dataCenter;
		private readonly IViewModelCommunication superViewModelCommunication;		

		public AddAppointmentDialogWindowBuilder(IDataCenter dataCenter,
												 IViewModelCommunication superViewModelCommunication)
		{
			this.dataCenter = dataCenter;
			this.superViewModelCommunication = superViewModelCommunication;			
		}

		public AddAppointmentDialog BuildWindow()
		{
			var creationDate      = superViewModelCommunication.GetGlobalViewModelVariable<Date>(AppointmentGridSelectedDateVariable).Value;
			var medicalPracticeId = superViewModelCommunication.GetGlobalViewModelVariable<Guid>(AppointmentGridDisplayedPracticeVariable).Value;

			IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
			IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
			IStateEngine viewModelStateEngine = new StateEngine();
			IViewModelCollections viewModelCollections = new ViewModelCollections();

			IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
																						viewModelStateEngine,
																						viewModelCollections);

			viewModelCommunication.RegisterGlobalViewModelVariable(SelectedPatientVariable, (Patient)null);

			var selectedPatientVariable = viewModelCommunication.GetGlobalViewModelVariable<Patient>(SelectedPatientVariable);

			IPatientSelectorViewModel patientSelectorViewModel = new PatientSelectorViewModel(dataCenter, selectedPatientVariable);

			return new AddAppointmentDialog
			       {
						Owner = Application.Current.MainWindow,
						DataContext = new AddAppointmentDialogViewModel(patientSelectorViewModel, 
																		viewModelCommunication, 
																		dataCenter, 
																		creationDate, 
																		medicalPracticeId)
			       };
		}

		public void DisposeWindow(AddAppointmentDialog buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}
