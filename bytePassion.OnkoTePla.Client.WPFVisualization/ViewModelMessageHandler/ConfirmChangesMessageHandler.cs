﻿using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler
{
	public class ConfirmChangesMessageHandler : IViewModelMessageHandler<ConfirmChanges>
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly IGlobalState<AppointmentModifications> currentModifiedAppointmentVariable; 

		public ConfirmChangesMessageHandler (IViewModelCommunication viewModelCommunication)
		{
			this.viewModelCommunication = viewModelCommunication;
			currentModifiedAppointmentVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable
			);
		}

		public void Process(ConfirmChanges message)
		{
			var currentMedicalPracticeId = viewModelCommunication.GetGlobalViewModelVariable<Guid>(
				AppointmentGridDisplayedPracticeVariable
			).Value;

			viewModelCommunication.SendTo(
				AppointmentGridViewModelCollection,
				new AggregateIdentifier(currentModifiedAppointmentVariable.Value.CurrentLocation.PlaceAndDate.Date, 
										currentMedicalPracticeId),
				new SendCurrentChangesToCommandBus()
			);

			currentModifiedAppointmentVariable.Value.Dispose();
			currentModifiedAppointmentVariable.Value = null;
		}
	}
}
