using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Contracts.Appointments;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler
{
	public class ConfirmChangesMessageHandler : IViewModelMessageHandler<ConfirmChanges>
	{		
		private readonly IGlobalState<Appointment> selectedAppointmentVariable; 

		public ConfirmChangesMessageHandler (IViewModelCommunication viewModelCommunication)
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
