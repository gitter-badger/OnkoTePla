using System;
using System.Collections.Generic;
using System.Windows;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActionFactory;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandHandler
{
	public class ReplaceAppointmentCommandHandler : IDomainCommandHandler<ReplaceAppointment>
	{
		private readonly IConnectionService connectionService;
		private readonly ISession session;
		private readonly IClientPatientRepository patientRepository;
		private readonly IClientMedicalPracticeRepository practiceRepository;
		private readonly IUserActionBuilder userActionBuilder;
		private readonly Action<string> errorCallback;


		public ReplaceAppointmentCommandHandler (IConnectionService connectionService,
												 ISession session,
												 IClientPatientRepository patientRepository,
												 IClientMedicalPracticeRepository practiceRepository,
												 IUserActionBuilder userActionBuilder,
												 Action<string> errorCallback)
		{
			this.connectionService = connectionService;
			this.session = session;
			this.patientRepository = patientRepository;
			this.practiceRepository = practiceRepository;
			this.userActionBuilder = userActionBuilder;
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
					else
					{
						if (command.ActionTag == ActionTag.RegularAction)
						{
							practiceRepository.RequestMedicalPractice(
								sourcePractice =>
								{
									practiceRepository.RequestMedicalPractice(
										destinationPractice =>
										{
											patientRepository.RequestPatient(
												patient =>
												{
													Application.Current.Dispatcher.Invoke(() =>
													{
														session.ReportUserAction(userActionBuilder.BuildReplacedAction(command,
																												       patient,
																												       sourcePractice.GetTherapyPlaceById(command.OriginalTherapyPlaceId),
																												       destinationPractice.GetTherapyPlaceById(command.NewTherapyPlaceId)));
													});													
												},
												command.PatientId,
												errorCallback	
											);
										},
										command.DestinationAggregateId.MedicalPracticeId,
										command.DestinationAggregateId.PracticeVersion,
										errorCallback
									);
								},
								command.SourceAggregateId.MedicalPracticeId,
								command.SourceAggregateId.PracticeVersion,
								errorCallback	
							);							
						}						
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
