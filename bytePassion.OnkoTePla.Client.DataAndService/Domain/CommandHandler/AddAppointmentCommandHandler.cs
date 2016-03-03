using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandHandler
{
	public class AddAppointmentCommandHandler : IDomainCommandHandler<AddAppointment>
	{		
		private readonly IConnectionService connectionService;
		private readonly ISession session;
		private readonly Action<string> errorCallback;


		public AddAppointmentCommandHandler(IConnectionService connectionService,
											ISession session,
											Action<string> errorCallback)
		{			
			this.connectionService   = connectionService;
			this.session = session;
			this.errorCallback = errorCallback;
		}

		public void Process(AddAppointment command)
		{
			if (session.LoggedInUser == null)
			{
				errorCallback("commands can only be processed when a user is logged in");
				return;
			}
				 
			var addedEvent = new AppointmentAdded(command.AggregateId,
												  command.AggregateVersion,
												  session.LoggedInUser.Id,
												  TimeTools.GetCurrentTimeStamp(),
												  command.ActionTag,
												  command.PatientId,
												  command.Description,
												  command.StartTime,
												  command.EndTime,
												  command.TherapyPlaceId,
												  command.AppointmentId);

			connectionService.TryAddEvents(
				addingWasSuccesscful =>
				{
					if (!addingWasSuccesscful)
					{
						errorCallback("adding events failed");
					}
					else
					{
						if (command.ActionTag == ActionTag.RegularAction)
						{
							// TODO
							//session.ReportUserAction();
						}
					}
				},
				new List<DomainEvent> { addedEvent },
				errorCallback
			);
			
		}
	}
}
