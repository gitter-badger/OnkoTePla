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
										 Guid therapyPlaceId,
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
							
							commandBus.SendCommand(new AddAppointment(aggregateId, 
																	  appointmentSet.AggregateVersion, 
																	  session.LoggedInUser.Id, 
																	  actionTag, 
																	  patientId, 
																	  description, 
																	  startTime, 
																	  endTime, 
																	  therapyPlaceId, 
																	  Guid.NewGuid()));



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
										  Guid patientId, string newDescription, Date newDate, Time newBeginTime, 
										  Time newEndTime, Guid newTherapyplaceId, Guid originalAppointmentId, 
										  Date originalDate, ActionTag actionTag, 
										  Action<string> errorCallback)
		{
			if (sourceLocation == destinationLocation)
				TryReplaceAppointmentWithinDay(sourceLocation, patientId, newDescription, newDate,newBeginTime, newEndTime, 
											   newTherapyplaceId, originalAppointmentId, actionTag, errorCallback);
			else
			{
				TryReplaceAppointmentBetweenDays(sourceLocation, destinationLocation, patientId, newDescription, newDate, 
												 newBeginTime, newEndTime, newTherapyplaceId, originalAppointmentId, 
												 originalDate, actionTag, errorCallback);
			}			
		}		

		private void TryReplaceAppointmentWithinDay(AggregateIdentifier location, Guid patientId, string newDescription, 
													Date newDate, Time newBeginTime, Time newEndTime, Guid newTherapyplaceId, 
													Guid originalAppointmentId, ActionTag actionTag,
													Action<string> errorCallback)
		{
			RequestLock(
				() =>
				{
					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!ReplacementIsPossible(location, location, newDate, newBeginTime, newEndTime, newTherapyplaceId, 
													   originalAppointmentId, newDate, appointmentSet, appointmentSet))
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
																		  newDescription, 
																		  newDate, 
																		  newBeginTime, 
																		  newEndTime, 
																		  newTherapyplaceId, 
																		  originalAppointmentId, 
																		  newDate));

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
													  Guid patientId, string newDescription, Date newDate, Time newBeginTime,
													  Time newEndTime, Guid newTherapyplaceId, Guid originalAppointmentId,
													  Date originalDate, ActionTag actionTag,
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

											if (!ReplacementIsPossible(sourceLocation, destinationLocation, newDate, newBeginTime, newEndTime, 
												newTherapyplaceId, originalAppointmentId, newDate, sourceAppointmentSet, destinationAppointmentSet))
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
																						  newDescription,
																						  newDate,
																						  newBeginTime,
																						  newEndTime,
																						  newTherapyplaceId,
																						  originalAppointmentId,
																						  originalDate));

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

		public void TryDeleteAppointment (AggregateIdentifier location, Guid appointmentId, Guid patientId,
										 ActionTag actionTag, Action<string> errorCallback)
		{
			RequestLock(
				() =>
				{
					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!DeletionPossible(appointmentId, appointmentSet))
							{
								errorCallback("termin kann nicht gelöscht werden, da nicht vorhanden");
								ReleaseAllLocks();
								return;
							}
							
							commandBus.SendCommand(new DeleteAppointment(location, 
																		 appointmentSet.AggregateVersion, 
																		 session.LoggedInUser.Id, 
																		 patientId,
																		 actionTag,
							                                             appointmentId));
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
