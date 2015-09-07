using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Contracts.Appointments;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler
{
	public class RejectChangesMessageHandler : IViewModelMessageHandler<RejectChanges>
	{
		private readonly IGlobalState<Appointment> selectedAppointmentVariable;

		public RejectChangesMessageHandler (ViewModelCommunication<ViewModelMessage> viewModelCommunication)
		{			
			selectedAppointmentVariable = viewModelCommunication.GetGlobalViewModelVariable<Appointment>(
				SelectedAppointmentVariable
			);
		}

		public void Process(RejectChanges message)
		{
			// TODO: proper implementation
			selectedAppointmentVariable.Value = null;
		}
	}
}
