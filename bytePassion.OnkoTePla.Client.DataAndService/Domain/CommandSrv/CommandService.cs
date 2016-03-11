using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Locking;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv
{
	public class CommandService : ICommandService
	{
		private readonly ISession session;
		private readonly IClientReadModelRepository readModelRepository;
		private readonly ICommandBus commandBus;

		private readonly IList<Lock> currentLocks;

		public CommandService(ISession session,
							  IClientReadModelRepository readModelRepository,
							  ICommandBus commandBus)
		{
			this.session = session;
			this.readModelRepository = readModelRepository;
			this.commandBus = commandBus;

			currentLocks = new List<Lock>();
		}

		public void TryAddNewAppointment(Action<bool> operationResultCallback, 
										 AggregateIdentifier aggregateId,
										 Guid patientId, string description,
										 Time startTime, Time endTime,
										 Guid therapyPlaceId, Guid appointmentId,
										 ActionTag actionTag,
										 Action<string> errorCallback)
		{
			RequestLock(
				successfulLocked =>
				{
					if (!successfulLocked)
					{
						operationResultCallback(false);
						return;
					}
								
					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!AddingIsPossible(therapyPlaceId, appointmentId, startTime, endTime, appointmentSet))
							{								
								ReleaseAllLocks(errorCallback);
								operationResultCallback(false);
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



							ReleaseAllLocks(errorCallback);
							operationResultCallback(true);
						},
						aggregateId,
						errorCallback	
					);										
				},
				aggregateId.MedicalPracticeId,
				aggregateId.Date,
				errorCallback
			);						
		}

		private static bool AddingIsPossible(Guid therapyPlaceId, Guid appointmentId,
											 Time startTime, Time endTime, 
											 FixedAppointmentSet appointmentSet)
		{			
			return appointmentSet.Appointments
								 .Where(appointment => appointment.TherapyPlace.Id == therapyPlaceId)
								 .Where(appointment => appointment.Id != appointmentId)
								 .All(appointment => (appointment.StartTime <= startTime || appointment.StartTime >= endTime) && 
													 (appointment.EndTime   <= startTime || appointment.EndTime   >= endTime));
		}

		
		public void TryReplaceAppointment(Action<bool> operationResultCallback,
										  AggregateIdentifier sourceLocation, AggregateIdentifier destinationLocation, 
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
				TryReplaceAppointmentWithinDay(operationResultCallback,
											   sourceLocation, patientId, 
											   originalDescription, newDescription,
											   originalDate,
											   originalStartTime, newStartTime,
											   originalEndTime, newEndTime,
											   originalTherapyPlaceId, newTherapyPlaceId,
											   originalAppointmendId,
											   actionTag, errorCallback);
			else
			{
				TryReplaceAppointmentBetweenDays(operationResultCallback,
												 sourceLocation, destinationLocation, patientId,
												 originalDescription, newDescription,
											     originalDate, newDate,
											     originalStartTime, newStartTime,
											     originalEndTime, newEndTime,
											     originalTherapyPlaceId, newTherapyPlaceId,
											     originalAppointmendId,
												 actionTag, errorCallback);
			}			
		}		
		
		private void TryReplaceAppointmentWithinDay(Action<bool> operationResultCallback,
													AggregateIdentifier location, Guid patientId,
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
				successfulLocked =>
				{
					if (!successfulLocked)
					{
						operationResultCallback(false);
						return;
					}

					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!ReplacementIsPossible(newStartTime, newEndTime, newTherapyPlaceId,
													   originalAppointmendId, appointmentSet, appointmentSet))
							{								
								ReleaseAllLocks(errorCallback);
								operationResultCallback(false);
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

							ReleaseAllLocks(errorCallback);
							operationResultCallback(true);
						},
						location,
						errorCallback
					);
				},
				location.MedicalPracticeId,
				location.Date,
				errorCallback				
			);
		}
		
		private void TryReplaceAppointmentBetweenDays(Action<bool> operationResultCallback,
													  AggregateIdentifier sourceLocation, AggregateIdentifier destinationLocation,
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
				successfulLockedSource =>
				{
					if (!successfulLockedSource)
					{
						operationResultCallback(false);
						return;
					}

					readModelRepository.RequestAppointmentSetOfADay(
						sourceAppointmentSet =>
						{
							RequestLock(
								successfulLockedDestination =>
								{
									if (!successfulLockedDestination)
									{
										ReleaseAllLocks(errorCallback);
										operationResultCallback(false);
										return;
									}

									readModelRepository.RequestAppointmentSetOfADay(
										destinationAppointmentSet =>
										{

											if (!ReplacementIsPossible(newStartTime, newEndTime, newTherapyPlaceId,
																	   originalAppointmendId, sourceAppointmentSet, destinationAppointmentSet))
											{												
												ReleaseAllLocks(errorCallback);
												operationResultCallback(false);
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

											ReleaseAllLocks(errorCallback);
											operationResultCallback(true);
										},
										destinationLocation,
										errorCallback
									);
								},
								destinationLocation.MedicalPracticeId,
								destinationLocation.Date,
								errorMsg =>
								{
									errorCallback(errorMsg);
									ReleaseAllLocks(errorCallback);
								}
							);
						}, 
						sourceLocation,
						errorCallback
					);
				},
				sourceLocation.MedicalPracticeId,
				sourceLocation.Date,
				errorCallback
			);
		}

		private static bool ReplacementIsPossible(Time newBeginTime, Time newEndTime, Guid newTherapyplaceId,
												  Guid originalAppointmentId,
												  FixedAppointmentSet sourceAppointmentSet, FixedAppointmentSet destinationAppointmentSet)
		{
			return DeletionPossible(originalAppointmentId, sourceAppointmentSet) && 
				   AddingIsPossible(newTherapyplaceId, originalAppointmentId, newBeginTime, newEndTime, destinationAppointmentSet);
		}


		public void TryDeleteAppointment (Action<bool> operationResultCallback, 
										  AggregateIdentifier location, Guid patientId, Guid removedAppointmentId,
										  string removedAppointmentDescription, Time removedAppointmentStartTime, Time removedAppointmentEndTime,
										  Guid removedAppointmentTherapyPlaceId, ActionTag actionTag, Action<string> errorCallback)
		{
			RequestLock(
				successfulLocked =>
				{
					if (!successfulLocked)
					{
						operationResultCallback(false);
						return;
					}

					readModelRepository.RequestAppointmentSetOfADay(
						appointmentSet =>
						{
							if (!DeletionPossible(removedAppointmentId, appointmentSet))
							{
								errorCallback("termin kann nicht gelöscht werden, da nicht vorhanden");
								ReleaseAllLocks(errorCallback);
								operationResultCallback(false);
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
							ReleaseAllLocks(errorCallback);
							operationResultCallback(true);
						},
						location,
						errorCallback
						);
				},
				location.MedicalPracticeId,
				location.Date,
				errorCallback				
			);
		}
		

		private static bool DeletionPossible(Guid appointmentId, FixedAppointmentSet appointmentSet)
		{
			return appointmentSet.Appointments.Any(appointment => appointment.Id == appointmentId);
		}		

		private void RequestLock(Action<bool> lockingResult, Guid medicalPracticeId, Date day, Action<string> errorCallback)
		{
			session.TryToGetLock(
				lockingSuccessful =>
				{
					if (lockingSuccessful)
					{
						currentLocks.Add(new Lock(medicalPracticeId, day));
					}
					lockingResult(lockingSuccessful);
				}, 
				medicalPracticeId, 
				day, 
				errorCallback
			);			
		}
		
		private void ReleaseAllLocks(Action<string> errorCallback)
		{
			foreach (var @lock in currentLocks)
			{
				session.ReleaseLock(() =>
				{
					currentLocks.Remove(@lock);
				}, 
				@lock.MedicalPracticeId, 
				@lock.Day, 
				errorCallback);
			}
		}
	}
}
