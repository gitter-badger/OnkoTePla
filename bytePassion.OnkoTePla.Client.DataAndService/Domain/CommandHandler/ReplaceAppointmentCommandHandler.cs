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
	public class ReplaceAppointmentCommandHandler : IDomainCommandHandler<ReplaceAppointment>
	{
		private readonly IConnectionService connectionService;
		private readonly ISession session;
		private readonly Action<string> errorCallback;


		public ReplaceAppointmentCommandHandler (IConnectionService connectionService,
												 ISession session,
												 Action<string> errorCallback)
		{
			this.connectionService = connectionService;
			this.session = session;
			this.errorCallback = errorCallback;
		}

		
		public void Process(ReplaceAppointment command)
		{
			if (session.LoggedInUser == null)
			{
				errorCallback("commands can only be processed when a user is logged in");
				return;
			}

			var eventList = new List<DomainEvent>();

			if (command.NewDate == command.OriginalDate)
			{
				
				eventList.Add(new AppointmentReplaced(command.SourceAggregateId,										
													  command.SourceAggregateVersion,
													  session.LoggedInUser.Id,
													  command.PatientId,
													  TimeTools.GetCurrentTimeStamp(),
													  command.ActionTag,
													  command.NewDescription,
													  command.NewDate,
													  command.NewStartTime,
													  command.NewEndTime,
													  command.NewTherapyPlaceId,
													  command.OriginalAppointmendId));				
			}
			else
			{
				eventList.Add(new AppointmentDeleted(command.SourceAggregateId, 
													 command.SourceAggregateVersion, 
													 session.LoggedInUser.Id, 
													 command.PatientId, 
													 TimeTools.GetCurrentTimeStamp(), 
													 GetDividedActionTag(command.ActionTag),
													 command.OriginalAppointmendId));

				eventList.Add(new AppointmentAdded(command.DestinationAggregateId, 
												   command.DestinationAggregateVersion, 
												   session.LoggedInUser.Id, 
												   TimeTools.GetCurrentTimeStamp(),
												   GetDividedActionTag(command.ActionTag), 
												   command.PatientId, 
												   command.NewDescription, 
												   command.NewStartTime, 
												   command.NewEndTime, 
												   command.NewTherapyPlaceId, 
												   command.OriginalAppointmendId));				
			}

			connectionService.TryAddEvents(
				addingWasSuccesscful =>
				{
					if (!addingWasSuccesscful)
					{
						errorCallback("adding events failed");
					}
				},
				eventList,
				errorCallback
			);
		}

		private static ActionTag GetDividedActionTag (ActionTag actionTag)
		{
			switch (actionTag)
			{
				case ActionTag.RegularAction: { return ActionTag.RegularDividedReplaceAction; }
				case ActionTag.RedoAction:    { return ActionTag.RedoDividedReplaceAction;    }
				case ActionTag.UndoAction:    { return ActionTag.UndoDividedReplaceAction;    }
			}

			throw new ArgumentException("internal parameter error");
		}
	}
}
