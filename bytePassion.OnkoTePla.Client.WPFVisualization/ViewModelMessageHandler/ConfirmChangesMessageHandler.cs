﻿using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler
{
	public class ConfirmChangesMessageHandler : IViewModelMessageHandler<ConfirmChanges>
	{		
		private readonly IGlobalState<AppointmentModifications> currentModifiedAppointmentVariable; 

		public ConfirmChangesMessageHandler (IViewModelCommunication viewModelCommunication)
		{			
			currentModifiedAppointmentVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable
			);
		}
		
		public void Process(ConfirmChanges message)
		{
			// TODO: proper implementation
			currentModifiedAppointmentVariable.Value = null;
		}
	}
}
