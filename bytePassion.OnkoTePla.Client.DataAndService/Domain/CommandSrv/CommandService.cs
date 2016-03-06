using System;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv
{
	public class CommandService : ICommandService
	{
		private readonly ISession session;
		private readonly IClientReadModelRepository readModelRepository;
		private readonly ICommandBus commandBus;

		public CommandService(ISession session,
							  IClientReadModelRepository readModelRepository,
							  ICommandBus commandBus)
		{
			this.session = session;
			this.readModelRepository = readModelRepository;
			this.commandBus = commandBus;
		}

		public void TryAddNewAppointment(AggregateIdentifier aggregateId,
										 Guid patientId, string description,
										 Time startTime, Time endTime,
										 Guid therapyPlaceId, Guid appointmentId,
										 ActionTag actionTag,
										 Action<string> errorCallback)
		{
			RequestLock(
				() =>
				{					
					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!AddingIsPossible(aggregateId, therapyPlaceId, startTime, endTime, appointmentSet))
							{
								errorCallback("termin kann aufgrund von konflikten nicht angelegt werden");
								ReleaseAllLocks();
								return;
							}
							
							commandBus.SendCommand(new AddAppointment(appointmentSet.Identifier, 
																	  appointmentSet.AggregateVersion, 
																	  session.LoggedInUser.Id, 
																	  actionTag, 
																	  patientId, 
																	  description, 
																	  startTime, 
																	  endTime, 
																	  therapyPlaceId,
																	  appointmentId));



							ReleaseAllLocks();
						},
						aggregateId,
						errorCallback	
					);										
				},
				errorMsg => errorCallback($"lock is in use: {errorMsg}"),
				aggregateId.Date	
			);						
		}

		private static bool AddingIsPossible(AggregateIdentifier aggregateId, Guid therapyPlaceId,
											 Time startTime, Time endTime, 
											 FixedAppointmentSet appointmentSet)
		{
			// TODO
			return true;
		}


		public void TryReplaceAppointment(AggregateIdentifier sourceLocation, AggregateIdentifier destinationLocation, 
										  Guid patientId, 
										  string originalDescription, string newDescription,
										  Date   originalDate,          Date newDate,
										  Time   originalStartTime,     Time newStartTime,
										  Time   originalEndTime,       Time newEndTime,
										  Guid   originalTherapyPlaceId,Guid newTherapyPlaceId,
										  Guid   originalAppointmendId,
										  ActionTag actionTag, 
										  Action<string> errorCallback)
		{
			if (sourceLocation == destinationLocation)
				TryReplaceAppointmentWithinDay(sourceLocation, patientId, 
											   originalDescription, newDescription,
											   originalDate,
											   originalStartTime, newStartTime,
											   originalEndTime, newEndTime,
											   originalTherapyPlaceId, newTherapyPlaceId,
											   originalAppointmendId,
											   actionTag, errorCallback);
			else
			{
				TryReplaceAppointmentBetweenDays(sourceLocation, destinationLocation, patientId,
												 originalDescription, newDescription,
											     originalDate, newDate,
											     originalStartTime, newStartTime,
											     originalEndTime, newEndTime,
											     originalTherapyPlaceId, newTherapyPlaceId,
											     originalAppointmendId,
												 actionTag, errorCallback);
			}			
		}		

		private void TryReplaceAppointmentWithinDay(AggregateIdentifier location, Guid patientId,
													string originalDescription,  string newDescription,
													Date   date,
													Time   originalStartTime,      Time newStartTime,
													Time   originalEndTime,        Time newEndTime,
													Guid   originalTherapyPlaceId, Guid newTherapyPlaceId,
													Guid   originalAppointmendId,
													ActionTag actionTag,
													Action<string> errorCallback)
		{
			RequestLock(
				() =>
				{
					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!ReplacementIsPossible(location, location, date, newStartTime, newEndTime, newTherapyPlaceId,
													   originalAppointmendId, date, appointmentSet, appointmentSet))
							{
								errorCallback("termin kann aufgrund von konflikten nicht verschoben werden");
								ReleaseAllLocks();
								return;
							}
							
							commandBus.SendCommand(new ReplaceAppointment(appointmentSet.Identifier,
																		  appointmentSet.Identifier, 
																		  appointmentSet.AggregateVersion, 
																		  appointmentSet.AggregateVersion, 
																		  session.LoggedInUser.Id, 
																		  patientId, 
																		  actionTag, 
																		  originalDescription,
																		  newDescription, 
																		  date, 
																		  date,
																		  originalStartTime,
																		  newStartTime,
																		  originalEndTime,
																		  newEndTime,
																		  originalTherapyPlaceId,
																		  newTherapyPlaceId,
																		  originalAppointmendId));

							ReleaseAllLocks();
						},
						location,
						errorCallback
					);
				},
				errorMsg => errorCallback($"lock is in use: {errorMsg}"),
				location.Date
			);
		}

		private void TryReplaceAppointmentBetweenDays(AggregateIdentifier sourceLocation, AggregateIdentifier destinationLocation,
													  Guid patientId,
													  string originalDescription, string newDescription,
													  Date originalDate, Date newDate,
													  Time originalStartTime, Time newStartTime,
													  Time originalEndTime, Time newEndTime,
													  Guid originalTherapyPlaceId, Guid newTherapyPlaceId,
													  Guid originalAppointmendId,
													  ActionTag actionTag,
													  Action<string> errorCallback)
		{
			RequestLock(
				() =>
				{
					readModelRepository.RequestAppointmentSetOfADay(
						sourceAppointmentSet =>
						{

							RequestLock(
								() =>
								{
									readModelRepository.RequestAppointmentSetOfADay(
										destinationAppointmentSet =>
										{

											if (!ReplacementIsPossible(sourceLocation, destinationLocation, newDate, newStartTime, newEndTime, newTherapyPlaceId,
																	   originalAppointmendId, originalDate, sourceAppointmentSet, destinationAppointmentSet))
											{
												errorCallback("termin kann aufgrund von konflikten nicht verschoben werden");
												ReleaseAllLocks();
												return;
											}
											
											commandBus.SendCommand(new ReplaceAppointment(sourceAppointmentSet.Identifier,
																						  destinationAppointmentSet.Identifier,
																						  sourceAppointmentSet.AggregateVersion,
																						  destinationAppointmentSet.AggregateVersion,
																						  session.LoggedInUser.Id,
																						  patientId,
																						  actionTag,
																						  originalDescription,
																						  newDescription,
																						  originalDate,
																						  newDate,
																						  originalStartTime,
																						  newStartTime,
																						  originalEndTime,
																						  newEndTime,
																						  originalTherapyPlaceId,
																						  newTherapyPlaceId,
																						  originalAppointmendId));

											ReleaseAllLocks();

										},
										destinationLocation,
										errorCallback
									);
								},
								errorMsg =>
								{
									errorCallback($"lock is in use: {errorMsg}");
									ReleaseAllLocks();
								},
								destinationLocation.Date
							);
						}, 
						sourceLocation,
						errorCallback
					);
				},
				errorMsg => errorCallback($"lock is in use: {errorMsg}"),
				sourceLocation.Date
			);
		}

		private static bool ReplacementIsPossible(AggregateIdentifier sourceLocation, AggregateIdentifier destinationLocation,
												  Date newDate, Time newBeginTime, Time newEndTime, Guid newTherapyplaceId,
												  Guid originalAppointmentId, Date originalDate,
												  FixedAppointmentSet sourceAppointmentSet, FixedAppointmentSet destinationAppointmentSet)
		{
			// TODO
			return true;
		}


		public void TryDeleteAppointment (AggregateIdentifier location, Guid patientId, Guid removedAppointmentId,
										  string removedAppointmentDescription, Time removedAppointmentStartTime, Time removedAppointmentEndTime,
										  Guid removedAppointmentTherapyPlaceId, ActionTag actionTag, Action<string> errorCallback)
		{
			RequestLock(
				() =>
				{
					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!DeletionPossible(removedAppointmentId, appointmentSet))
							{
								errorCallback("termin kann nicht gelöscht werden, da nicht vorhanden");
								ReleaseAllLocks();
								return;
							}

							commandBus.SendCommand(new DeleteAppointment(appointmentSet.Identifier,
																		 appointmentSet.AggregateVersion,
																		 session.LoggedInUser.Id,
																		 patientId,
																		 actionTag,
																		 removedAppointmentId,
																		 removedAppointmentDescription,
																		 removedAppointmentStartTime,
																		 removedAppointmentEndTime,
																		 removedAppointmentTherapyPlaceId));
							ReleaseAllLocks();
						},
						location,
						errorCallback
						);
				},
				errorMsg => errorCallback($"lock is in use: {errorMsg}"),
				location.Date
			);
		}
		

		private static bool DeletionPossible(Guid appointmentId, FixedAppointmentSet appointmentSet)
		{
			return appointmentSet.Appointments.Any(appointment => appointment.Id == appointmentId);
		}

		private void RequestLock(Action lockGranted, Action<string> lockDenied, Date day)
		{
			// TODO: Lock day
			lockGranted();
		}

		private void ReleaseAllLocks()
		{
			// TODO: release lock
		}
	}
}
