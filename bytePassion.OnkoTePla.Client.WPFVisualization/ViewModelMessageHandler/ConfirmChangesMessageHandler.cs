using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Contracts.Appointments;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler
{
	public class ConfirmChangesMessageHandler : IViewModelMessageHandler<ConfirmChanges>
	{		
		private readonly IGlobalState<Appointment> selectedAppointmentVariable; 

		public ConfirmChangesMessageHandler (ViewModelCommunication<ViewModelMessage> viewModelCommunication)
		{			
			selectedAppointmentVariable = viewModelCommunication.GetGlobalViewModelVariable<Appointment>(
				SelectedAppointmentVariable
			);
		}
		
		public void Process(ConfirmChanges message)
		{
			// TODO: proper implementation
			selectedAppointmentVariable.Value = null;
		}
	}
}
