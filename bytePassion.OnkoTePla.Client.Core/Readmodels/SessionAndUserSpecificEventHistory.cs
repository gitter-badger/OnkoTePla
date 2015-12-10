﻿using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Readmodels.Helper;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Contracts.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class SessionAndUserSpecificEventHistory : ReadModelBase,
													  IUndoRedo													  
	{
		private class InitialDummyEvent : DomainEvent
		{
			public InitialDummyEvent()
				: base(new AggregateIdentifier(Date.Dummy, new Guid()), 0, new Guid(), new Guid(), null, ActionTag.RegularAction)
			{				
			}
		}


		private readonly ICommandBus commandBus;
		private readonly IReadModelRepository readModelRepository;
	    private readonly IPatientReadRepository patientRepository;
	    private readonly IConfigurationReadRepository configuration;
	    private readonly User currentUser;		
		private readonly uint maximalSavedVersions;
		private readonly LinkedList<DomainEvent> userTriggeredEvents;

		private LinkedListNode<DomainEvent> eventPointer;

		private bool undoPossible;
		private bool redoPossible;

		public SessionAndUserSpecificEventHistory(IEventBus eventBus,
												  ICommandBus commandBus,
												  IReadModelRepository readModelRepository,
                                                  IPatientReadRepository patientRepository,
                                                  IConfigurationReadRepository configuration,
												  User currentUser,
												  uint maximalSavedVersions) 
			: base(eventBus)
		{
			this.commandBus = commandBus;
			this.readModelRepository = readModelRepository;
		    this.patientRepository = patientRepository;
		    this.configuration = configuration;
		    this.currentUser = currentUser;			
			this.maximalSavedVersions = maximalSavedVersions;

			userTriggeredEvents = new LinkedList<DomainEvent>();

			var initialNode = new LinkedListNode<DomainEvent>(new InitialDummyEvent());

			userTriggeredEvents.AddLast(initialNode);
			eventPointer = initialNode;
			

			CheckIfUndoAndRedoIsPossible();
		}


		public bool UndoPossible
		{
			get { return undoPossible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref undoPossible, value); }
		}

		public bool RedoPossible
		{
			get { return redoPossible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref redoPossible, value); }
		}

		private void TryAddEvent (DomainEvent domainEvent)
		{
			if (domainEvent.UserId == currentUser.Id && (domainEvent.ActionTag == ActionTag.RegularAction ||
			                                             domainEvent.ActionTag == ActionTag.RegularDividedReplaceAction))
			{
				var newVersionNode = new LinkedListNode<DomainEvent>(domainEvent);

				RemoveAllFromEndTo(eventPointer);
				userTriggeredEvents.AddLast(newVersionNode);
				eventPointer = newVersionNode;

				if (userTriggeredEvents.Count == maximalSavedVersions + 1)
					userTriggeredEvents.RemoveFirst();
			}

			CheckIfUndoAndRedoIsPossible();
		}

	    public string GetRedoActionMessage()
	    {
            if (!RedoPossible)
                throw new InvalidOperationException("redo not possible");

	        string result ="";

            var domainEventToBeRestored = eventPointer.Next.Value;
            var readModel = readModelRepository.GetAppointmentsOfADayReadModel(domainEventToBeRestored.AggregateId);

            switch (domainEventToBeRestored.ActionTag)
            {
                case ActionTag.RegularAction:
                    {
                        if (domainEventToBeRestored.GetType() == typeof(AppointmentAdded))
                        {
                            var addAppointmentEventToBeRestored = (AppointmentAdded)domainEventToBeRestored;

                            result = RedoStringGenerator.ForAddedEvent(patientRepository.GetPatientById(addAppointmentEventToBeRestored.PatientId),
                                                                       addAppointmentEventToBeRestored.CreateAppointmentData.Day,
                                                                       addAppointmentEventToBeRestored.CreateAppointmentData.StartTime,
                                                                       addAppointmentEventToBeRestored.CreateAppointmentData.EndTime);
                           
                        }
                        else if (domainEventToBeRestored.GetType() == typeof(AppointmentReplaced))
                        {
                            var replacedEventToBeRestored = (AppointmentReplaced)domainEventToBeRestored;

                            var currentVersionOfAppointment = readModel.Appointments.First(
                                appointment => appointment.Id == replacedEventToBeRestored.OriginalAppointmendId
                            );

                           result = RedoStringGenerator.ForReplacedEvent(patientRepository.GetPatientById(replacedEventToBeRestored.PatientId), 
																		 replacedEventToBeRestored.NewDate,
                                                                         currentVersionOfAppointment.StartTime,
                                                                         currentVersionOfAppointment.EndTime,
                                                                         currentVersionOfAppointment.TherapyPlace,
                                                                         replacedEventToBeRestored.NewStartTime,
                                                                         replacedEventToBeRestored.NewEndTime,
                                                                         configuration.GetMedicalPracticeByIdAndVersion(readModel.Identifier.MedicalPracticeId,
                                                                                                                        readModel.Identifier.PracticeVersion)
                                                                                      .GetTherapyPlaceById(replacedEventToBeRestored.NewTherapyPlaceId)
                                                                         );
                        }
                        else if (domainEventToBeRestored.GetType() == typeof(AppointmentDeleted))
                        {
                            var deletedEventToBeRestored = (AppointmentDeleted) domainEventToBeRestored;

                            var currentVersionOfAppointment = readModel.Appointments.First(
                                appointment => appointment.Id == deletedEventToBeRestored.RemovedAppointmentId
                            );

                            result = RedoStringGenerator.ForDeletedEvent(patientRepository.GetPatientById(deletedEventToBeRestored.PatientId),
                                                                         currentVersionOfAppointment.Day,
                                                                         currentVersionOfAppointment.StartTime,
                                                                         currentVersionOfAppointment.EndTime);

                        }
                        else
                            throw new Exception("internal error");
                       
                        break;
                    }

                case ActionTag.RegularDividedReplaceAction:
                    {
                        var currentEvent = eventPointer.Next.Value;
                        var nextEvent = eventPointer.Next.Next.Value;

                        if (currentEvent.GetType() != typeof(AppointmentDeleted) || nextEvent.GetType() != typeof(AppointmentAdded))
                            throw new Exception("internal error");

                        var addedEvent   = (AppointmentAdded)nextEvent;
                        var deletedEvent = (AppointmentDeleted)currentEvent;

                        var readModelWhereTheAppointmentIsToBeAdded   = readModelRepository.GetAppointmentsOfADayReadModel(addedEvent.AggregateId);
                        var readModelWhereTheAppointmentIsToBeDeleted = readModelRepository.GetAppointmentsOfADayReadModel(deletedEvent.AggregateId);

                        var currentVersionOfAppointment = readModelWhereTheAppointmentIsToBeDeleted.Appointments.First(
                            appointment => appointment.Id == deletedEvent.RemovedAppointmentId
                        );

                        result = RedoStringGenerator.ForDividedReplacedEvent(patientRepository.GetPatientById(addedEvent.PatientId),
                                                                             currentVersionOfAppointment.Day,
                                                                             addedEvent.CreateAppointmentData.Day,
                                                                             currentVersionOfAppointment.StartTime,
                                                                             addedEvent.CreateAppointmentData.StartTime,
                                                                             currentVersionOfAppointment.EndTime,
                                                                             addedEvent.CreateAppointmentData.EndTime,
                                                                             currentVersionOfAppointment.TherapyPlace,
                                                                             configuration.GetMedicalPracticeByIdAndVersion(readModelWhereTheAppointmentIsToBeAdded.Identifier.MedicalPracticeId,
                                                                                                                            readModelWhereTheAppointmentIsToBeAdded.Identifier.PracticeVersion)
                                                                                          .GetTherapyPlaceById(addedEvent.CreateAppointmentData.TherapyPlaceId)
                                                                             );

                        readModelWhereTheAppointmentIsToBeAdded.Dispose();
                        readModelWhereTheAppointmentIsToBeDeleted.Dispose();                        
                        break;
                    }
            }

            readModel.Dispose();

	        return result;
	    }

		public string GetUndoActionMessage()
		{
			if (!UndoPossible)
                throw new InvalidOperationException("undo not possible");

		    string result;

            var domainEvent = eventPointer.Value;

            switch (domainEvent.ActionTag)
            {
                case ActionTag.RegularAction:
                {
                    var readModel = readModelRepository.GetAppointmentsOfADayReadModel(domainEvent.AggregateId);

                    if (domainEvent.GetType() == typeof(AppointmentAdded))
                    {
                        var addedEvent = (AppointmentAdded)domainEvent;

                        result = UndoStringGenerator.ForAddedEvent(patientRepository.GetPatientById(addedEvent.PatientId),
                                                                 readModel.Identifier.Date,
                                                                 addedEvent.CreateAppointmentData.StartTime,
                                                                 addedEvent.CreateAppointmentData.EndTime);                                                       
                    }
                    else if (domainEvent.GetType() == typeof(AppointmentReplaced))
                    {
                        var replacedEvent = (AppointmentReplaced)domainEvent;

                        var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModel.Identifier,
                                                                                              eventPointer.Value.AggregateVersion - 1);
                                                
                        var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
                            appointment => appointment.Id == replacedEvent.OriginalAppointmendId
                        );

                        result = UndoStringGenerator.ForReplacedEvent(patientRepository.GetPatientById(replacedEvent.PatientId),
                                                                      readModel.Identifier.Date,
                                                                      replacedEvent.NewStartTime,
                                                                      replacedEvent.NewEndTime,
                                                                      configuration.GetMedicalPracticeByIdAndVersion(readModel.Identifier.MedicalPracticeId, 
                                                                                                                     readModel.Identifier.PracticeVersion)
																				   .GetTherapyPlaceById(replacedEvent.NewTherapyPlaceId),
                                                                      lastVersionOfTheAppointment.StartTime,
                                                                      lastVersionOfTheAppointment.EndTime,
                                                                      lastVersionOfTheAppointment.TherapyPlace);                                                        
                    }
                    else if (domainEvent.GetType() == typeof(AppointmentDeleted))
                    {
                        var deletedEvent = (AppointmentDeleted)domainEvent;

                        var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModel.Identifier,
                                                                                                eventPointer.Value.AggregateVersion - 1);

                        var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
                            appointment => appointment.Id == deletedEvent.RemovedAppointmentId
                        );

                        result = UndoStringGenerator.ForDeletedEvent(patientRepository.GetPatientById(deletedEvent.PatientId),
                                                                     readModel.Identifier.Date,
                                                                     lastVersionOfTheAppointment.StartTime,
                                                                     lastVersionOfTheAppointment.EndTime);
                    }
                    else
                    {
                        throw new Exception("internal error");
                    }

                    readModel.Dispose();                    
                    break;
                }
                case ActionTag.RegularDividedReplaceAction:
                {
                    var currentEvent = eventPointer.Value;
                    var lastEvent = eventPointer.Previous.Value;

                    if (currentEvent.GetType() != typeof(AppointmentAdded) || lastEvent.GetType() != typeof(AppointmentDeleted))
                        throw new Exception("internal error");

                    var addedEvent = (AppointmentAdded)currentEvent;
                    var deletedEvent = (AppointmentDeleted)lastEvent;

                    var readModelWhereTheAppointmentWasAdded = readModelRepository.GetAppointmentsOfADayReadModel(addedEvent.AggregateId);
                    var readModelWhereTheAppointmentWasDeleted = readModelRepository.GetAppointmentsOfADayReadModel(deletedEvent.AggregateId);

                    var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModelWhereTheAppointmentWasDeleted.Identifier,
                                                                                          eventPointer.Previous.Value.AggregateVersion - 1);

                    var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
                            appointment => appointment.Id == deletedEvent.RemovedAppointmentId
                    );

                    result = UndoStringGenerator.ForDividedReplacedEvent(patientRepository.GetPatientById(deletedEvent.PatientId),
                                                                         addedEvent.CreateAppointmentData.Day,       lastVersionOfTheAppointment.Day,
                                                                         addedEvent.CreateAppointmentData.StartTime, lastVersionOfTheAppointment.StartTime,
                                                                         addedEvent.CreateAppointmentData.EndTime,   lastVersionOfTheAppointment.EndTime,
                                                                         configuration.GetMedicalPracticeByIdAndVersion(readModelWhereTheAppointmentWasAdded.Identifier.MedicalPracticeId,
                                                                                                                     readModelWhereTheAppointmentWasAdded.Identifier.PracticeVersion)
                                                                          .GetTherapyPlaceById(addedEvent.CreateAppointmentData.TherapyPlaceId),
                                                                         lastVersionOfTheAppointment.TherapyPlace);
                    readModelWhereTheAppointmentWasAdded.Dispose();
                    readModelWhereTheAppointmentWasDeleted.Dispose();
                    break;
                }

                default:
                    throw new NotSupportedException();
            }

            return result;
		}

		public void Undo ()
		{			
			if (!UndoPossible)
				throw new InvalidOperationException("undo not possible");


			var domainEvent = eventPointer.Value;
			
			switch (domainEvent.ActionTag)
			{
				case ActionTag.RegularAction:
				{
					var readModel = readModelRepository.GetAppointmentsOfADayReadModel(domainEvent.AggregateId);

					if (domainEvent.GetType() == typeof(AppointmentAdded))
					{
						var addedEvent = (AppointmentAdded) domainEvent;
						
						commandBus.SendCommand(new DeleteAppointment(readModel.Identifier, 
																	 readModel.AggregateVersion,
																	 currentUser.Id,
																	 addedEvent.PatientId,
																	 ActionTag.UndoAction, 
																	 addedEvent.CreateAppointmentData.AppointmentId));
					}
					else if (domainEvent.GetType() == typeof(AppointmentReplaced))
					{
						var replacedEvent = (AppointmentReplaced) domainEvent;

						var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModel.Identifier, 
																							  eventPointer.Value.AggregateVersion-1);

						var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
							appointment => appointment.Id == replacedEvent.OriginalAppointmendId
						);

						commandBus.SendCommand(new ReplaceAppointment(readModel.Identifier,
																	  readModel.Identifier,
																	  readModel.AggregateVersion,
																	  readModel.AggregateVersion,
																	  currentUser.Id,
																	  replacedEvent.PatientId,
																	  ActionTag.UndoAction, 
																	  lastVersionOfTheAppointment.Description,
																	  lastVersionOfTheAppointment.Day,
																	  lastVersionOfTheAppointment.StartTime,
																	  lastVersionOfTheAppointment.EndTime,
																	  lastVersionOfTheAppointment.TherapyPlace.Id,
																	  lastVersionOfTheAppointment.Id,
																	  lastVersionOfTheAppointment.Day));

					}
					else if (domainEvent.GetType() == typeof(AppointmentDeleted))
					{
						var deletedEvent = (AppointmentDeleted) domainEvent;

						var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModel.Identifier,
																							  eventPointer.Value.AggregateVersion-1);

						var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
							appointment => appointment.Id == deletedEvent.RemovedAppointmentId
						);

						commandBus.SendCommand(new AddAppointment(readModel.Identifier,
																  readModel.AggregateVersion,
																  currentUser.Id,
																  ActionTag.UndoAction, 
																  deletedEvent.PatientId,
																  lastVersionOfTheAppointment.Description,
																  lastVersionOfTheAppointment.StartTime,
																  lastVersionOfTheAppointment.EndTime,
																  lastVersionOfTheAppointment.TherapyPlace.Id,
																  lastVersionOfTheAppointment.Id));
					}
					else
					{
						throw new Exception("internal error");
					}

					readModel.Dispose();
					eventPointer = eventPointer.Previous;
					break;
				}
				case ActionTag.RegularDividedReplaceAction:
				{					
					var currentEvent = eventPointer.Value;
					var lastEvent = eventPointer.Previous.Value;

					if (currentEvent.GetType() != typeof (AppointmentAdded) || lastEvent.GetType() != typeof (AppointmentDeleted))					
						throw new Exception("internal error");

					var addedEvent   = (AppointmentAdded) currentEvent;
					var deletedEvent = (AppointmentDeleted) lastEvent;

					var readModelWhereTheAppointmentWasAdded   = readModelRepository.GetAppointmentsOfADayReadModel(addedEvent.AggregateId);
					var readModelWhereTheAppointmentWasDeleted = readModelRepository.GetAppointmentsOfADayReadModel(deletedEvent.AggregateId);

					var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModelWhereTheAppointmentWasDeleted.Identifier,
																						  eventPointer.Previous.Value.AggregateVersion-1);

					var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
							appointment => appointment.Id == deletedEvent.RemovedAppointmentId
					);

					commandBus.SendCommand(new DeleteAppointment(readModelWhereTheAppointmentWasAdded.Identifier,
																 readModelWhereTheAppointmentWasAdded.AggregateVersion,
																 currentUser.Id,
																 addedEvent.PatientId,
																 ActionTag.UndoDividedReplaceAction, 
																 addedEvent.CreateAppointmentData.AppointmentId));

					commandBus.SendCommand(new AddAppointment(readModelWhereTheAppointmentWasDeleted.Identifier,
															  readModelWhereTheAppointmentWasDeleted.AggregateVersion,
															  currentUser.Id,
															  ActionTag.UndoDividedReplaceAction, 
															  deletedEvent.PatientId,
															  lastVersionOfTheAppointment.Description,
															  lastVersionOfTheAppointment.StartTime,
															  lastVersionOfTheAppointment.EndTime,
															  lastVersionOfTheAppointment.TherapyPlace.Id,
															  lastVersionOfTheAppointment.Id));


					readModelWhereTheAppointmentWasAdded.Dispose();
					readModelWhereTheAppointmentWasDeleted.Dispose();
					eventPointer = eventPointer.Previous.Previous;
					break;
				}			   
			       
			    default:
			        throw new NotSupportedException();
			}

		    CheckIfUndoAndRedoIsPossible();
		}

	    public void Redo()
	    {
	        if (!RedoPossible)
	            throw new InvalidOperationException("redo not possible");

	        var domainEventToBeRestored = eventPointer.Next.Value;
	        var readModel = readModelRepository.GetAppointmentsOfADayReadModel(domainEventToBeRestored.AggregateId);

	        switch (domainEventToBeRestored.ActionTag)
	        {
	            case ActionTag.RegularAction:
	            {
	                if (domainEventToBeRestored.GetType() == typeof (AppointmentAdded))
	                {
	                    var addAppointmentEventToBeRestored = (AppointmentAdded) domainEventToBeRestored;

	                    commandBus.SendCommand(new AddAppointment(readModel.Identifier, readModel.AggregateVersion, currentUser.Id, ActionTag.RedoAction, addAppointmentEventToBeRestored.PatientId, addAppointmentEventToBeRestored.CreateAppointmentData.Description, addAppointmentEventToBeRestored.CreateAppointmentData.StartTime, addAppointmentEventToBeRestored.CreateAppointmentData.EndTime, addAppointmentEventToBeRestored.CreateAppointmentData.TherapyPlaceId, addAppointmentEventToBeRestored.CreateAppointmentData.AppointmentId));
	                }
	                else if (domainEventToBeRestored.GetType() == typeof (AppointmentReplaced))
	                {
	                    var replacedEventToBeRestored = (AppointmentReplaced) domainEventToBeRestored;

	                    commandBus.SendCommand(new ReplaceAppointment(readModel.Identifier, readModel.Identifier, readModel.AggregateVersion, readModel.AggregateVersion, currentUser.Id, replacedEventToBeRestored.PatientId, ActionTag.RedoAction, replacedEventToBeRestored.NewDescription, replacedEventToBeRestored.NewDate, replacedEventToBeRestored.NewStartTime, replacedEventToBeRestored.NewEndTime, replacedEventToBeRestored.NewTherapyPlaceId, replacedEventToBeRestored.OriginalAppointmendId, replacedEventToBeRestored.NewDate));
	                }
	                else if (domainEventToBeRestored.GetType() == typeof (AppointmentDeleted))
	                {
	                    var deletedEventToBeRestored = (AppointmentDeleted) domainEventToBeRestored;

	                    commandBus.SendCommand(new DeleteAppointment(readModel.Identifier, readModel.AggregateVersion, currentUser.Id, deletedEventToBeRestored.PatientId, ActionTag.RedoAction, deletedEventToBeRestored.RemovedAppointmentId));
	                }
	                else
	                    throw new Exception("internal error");
	                eventPointer = eventPointer.Next;
	                break;
	            }

	            case ActionTag.RegularDividedReplaceAction:
	            {
	                var currentEvent = eventPointer.Next.Value;
	                var nextEvent = eventPointer.Next.Next.Value;

	                if (currentEvent.GetType() != typeof (AppointmentDeleted) || nextEvent.GetType() != typeof (AppointmentAdded))
	                    throw new Exception("internal error");

	                var addedEvent = (AppointmentAdded) nextEvent;
	                var deletedEvent = (AppointmentDeleted) currentEvent;

	                var readModelWhereTheAppointmentIsToBeAdded = readModelRepository.GetAppointmentsOfADayReadModel(addedEvent.AggregateId);
	                var readModelWhereTheAppointmentIsToBeDeleted = readModelRepository.GetAppointmentsOfADayReadModel(deletedEvent.AggregateId);

	                commandBus.SendCommand(new DeleteAppointment(readModelWhereTheAppointmentIsToBeDeleted.Identifier, readModelWhereTheAppointmentIsToBeDeleted.AggregateVersion, currentUser.Id, deletedEvent.PatientId, ActionTag.RedoDividedReplaceAction, deletedEvent.RemovedAppointmentId));

	                commandBus.SendCommand(new AddAppointment(readModelWhereTheAppointmentIsToBeAdded.Identifier, readModelWhereTheAppointmentIsToBeAdded.AggregateVersion, currentUser.Id, ActionTag.RedoDividedReplaceAction, addedEvent.PatientId, addedEvent.CreateAppointmentData.Description, addedEvent.CreateAppointmentData.StartTime, addedEvent.CreateAppointmentData.EndTime, addedEvent.CreateAppointmentData.TherapyPlaceId, addedEvent.CreateAppointmentData.AppointmentId));

	                readModelWhereTheAppointmentIsToBeAdded.Dispose();
	                readModelWhereTheAppointmentIsToBeDeleted.Dispose();
	                eventPointer = eventPointer.Next.Next;
	                break;
	            }
	        }

	        readModel.Dispose();
	        CheckIfUndoAndRedoIsPossible();
	    }

	    private void CheckIfUndoAndRedoIsPossible()
	    {
	        if (eventPointer != null)
	        {
	            UndoPossible = eventPointer.Previous != null;
	            RedoPossible = eventPointer.Next != null;
	        }
	        else
	        {
	            UndoPossible = false;
	            RedoPossible = false;
	        }
	    }

	    private void RemoveAllFromEndTo(LinkedListNode<DomainEvent> node)
	    {
	        while (userTriggeredEvents.Last != node)
	        {
	            userTriggeredEvents.RemoveLast();

	            if (userTriggeredEvents.Count == 0)
	                throw new InvalidOperationException();
	        }
	    }


	    public override void Process(AppointmentAdded domainEvent)
	    {
	        TryAddEvent(domainEvent);
	    }

	    public override void Process(AppointmentReplaced domainEvent)
	    {
	        TryAddEvent(domainEvent);
	    }

	    public override void Process(AppointmentDeleted domainEvent)
	    {
	        TryAddEvent(domainEvent);
	    }


	    public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;

	    public event PropertyChangedEventHandler PropertyChanged;
	}

}
