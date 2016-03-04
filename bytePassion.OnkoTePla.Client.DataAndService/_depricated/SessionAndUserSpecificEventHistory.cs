//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Windows;
//using bytePassion.Lib.FrameworkExtensions;
//using bytePassion.Lib.TimeLib;
//using bytePassion.Lib.Utils;
//using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
//using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
//using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
//using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels;
//using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
//using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
//using bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository;
//using bytePassion.OnkoTePla.Contracts.Config;
//using bytePassion.OnkoTePla.Contracts.Domain;
//using bytePassion.OnkoTePla.Contracts.Domain.Events;
//using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
//
//namespace bytePassion.OnkoTePla.Client.DataAndService._depricated
//{
//	public class UndoRedoServiceOld : IUndoRedo													  
//	{
//		private class InitialDummyEvent : DomainEvent
//		{
//			public InitialDummyEvent()
//				: base(new AggregateIdentifier(Date.Dummy, new Guid()), 0, new Guid(), new Guid(), null, ActionTag.RegularAction)
//			{				
//			}
//		}
//
//
//		private readonly ICommandBus commandBus;
//		private readonly IClientReadModelRepository readModelRepository;
//		private readonly IClientPatientRepository patientRepository;
//		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
//	    private readonly ClientUserData currentUser;		
//		private readonly uint maximalSavedVersions;
//		private readonly Action<string> errorCallback;
//		private readonly LinkedList<DomainEvent> userTriggeredEvents;
//
//		private LinkedListNode<DomainEvent> eventPointer;
//
//		private bool undoPossible;
//		private bool redoPossible;
//		
//		public UndoRedoServiceOld (IClientEventBus eventBus,
//							   ICommandBus commandBus,
//							   IClientReadModelRepository readModelRepository,
//							   IClientPatientRepository patientRepository,
//							   IClientMedicalPracticeRepository medicalPracticeRepository,
//							   ClientUserData currentUser,
//							   uint maximalSavedVersions,
//							   Action<string> errorCallback) 			
//		{
//			this.commandBus = commandBus;
//			this.readModelRepository = readModelRepository;
//			this.patientRepository = patientRepository;
//			this.medicalPracticeRepository = medicalPracticeRepository;
//		    this.currentUser = currentUser;			
//			this.maximalSavedVersions = maximalSavedVersions;
//			this.errorCallback = errorCallback;
//
//			userTriggeredEvents = new LinkedList<DomainEvent>();
//
//			var initialNode = new LinkedListNode<DomainEvent>(new InitialDummyEvent());
//
//			userTriggeredEvents.AddLast(initialNode);
//			eventPointer = initialNode;
//			
//
//			CheckIfUndoAndRedoIsPossible();
//		}
//
//
//		public bool UndoPossible
//		{
//			get { return undoPossible; }
//			private set { PropertyChanged.ChangeAndNotify(this, ref undoPossible, value); }
//		}
//
//		public bool RedoPossible
//		{
//			get { return redoPossible; }
//			private set { PropertyChanged.ChangeAndNotify(this, ref redoPossible, value); }
//		}
//
//		private void TryAddEvent (DomainEvent domainEvent)
//		{
//			if (domainEvent.UserId == currentUser.Id && (domainEvent.ActionTag == ActionTag.RegularAction ||
//			                                             domainEvent.ActionTag == ActionTag.RegularDividedReplaceAction))
//			{
//				var newVersionNode = new LinkedListNode<DomainEvent>(domainEvent);
//
//				RemoveAllFromEndTo(eventPointer);
//				userTriggeredEvents.AddLast(newVersionNode);
//				eventPointer = newVersionNode;
//
//				if (userTriggeredEvents.Count == maximalSavedVersions + 1)
//					userTriggeredEvents.RemoveFirst();
//			}
//
//			CheckIfUndoAndRedoIsPossible();
//		}
//
//		#region Undo
//
//		public void Undo ()
//		{
//			if (!UndoPossible)
//				throw new InvalidOperationException("undo not possible");
//
//			UndoPossible = false;
//			RedoPossible = false;
//
//			var domainEvent = eventPointer.Value;
//
//			switch (domainEvent.ActionTag)
//			{
//				case ActionTag.RegularAction:
//				{
//					HandleNormalUndo(domainEvent);
//					break;
//				}
//				case ActionTag.RegularDividedReplaceAction:
//				{
//					HandleDividedUndo();
//					break;
//				}
//
//				default:
//					throw new NotSupportedException();
//			}			
//		}
//
//		#region Undo-Handler
//
//		private void HandleNormalUndo (DomainEvent domainEvent)
//		{
//			readModelRepository.RequestAppointmentSetOfADay(
//				currentAppointmentSet =>
//				{
//					if (domainEvent.GetType() == typeof(AppointmentAdded))
//					{
//						UndoRegularAddedEvent((AppointmentAdded)domainEvent, currentAppointmentSet);
//
//						Application.Current.Dispatcher.Invoke(() =>
//						{
//							eventPointer = eventPointer.Previous;
//							CheckIfUndoAndRedoIsPossible();
//						});						
//					}
//					else if (domainEvent.GetType() == typeof(AppointmentReplaced))
//					{
//						readModelRepository.RequestAppointmentSetOfADay(
//							beforeChangeAppointmentSet =>
//							{
//								UndoRegularReplacedEvent((AppointmentReplaced)domainEvent, currentAppointmentSet, beforeChangeAppointmentSet);
//
//								Application.Current.Dispatcher.Invoke(() =>
//								{
//									eventPointer = eventPointer.Previous;
//									CheckIfUndoAndRedoIsPossible();
//								});								
//							},
//							currentAppointmentSet.Identifier,
//							//eventPointer.Value.AggregateVersion-1,
//							errorCallback
//						);
//					}
//					else if (domainEvent.GetType() == typeof(AppointmentDeleted))
//					{
//						readModelRepository.RequestAppointmentSetOfADay(
//							beforeChangeAppointmentSet =>
//							{
//								UndoRegularDeletedEvent((AppointmentDeleted)domainEvent, currentAppointmentSet, beforeChangeAppointmentSet);
//
//								Application.Current.Dispatcher.Invoke(() =>
//								{
//									eventPointer = eventPointer.Previous;
//									CheckIfUndoAndRedoIsPossible();
//								});								
//							},
//							currentAppointmentSet.Identifier,
//							//eventPointer.Value.AggregateVersion-1,
//							errorCallback
//						);
//					}
//					else
//					{
//						throw new Exception("internal error");
//					}
//				},
//				domainEvent.AggregateId,
//				//uint.MaxValue,
//				errorCallback
//			);			
//		}
//
//		private void HandleDividedUndo()
//		{
//			var currentEvent = eventPointer.Value;
//			var lastEvent = eventPointer.Previous.Value;
//
//			if (currentEvent.GetType() != typeof(AppointmentAdded) || lastEvent.GetType() != typeof(AppointmentDeleted))
//				throw new Exception("internal error");
//
//			var addedEvent   = (AppointmentAdded)   currentEvent;
//			var deletedEvent = (AppointmentDeleted) lastEvent;
//
//			readModelRepository.RequestAppointmentSetOfADay(
//				appointmentSetWhereTheAppointmentWasAdded =>
//				{
//					readModelRepository.RequestAppointmentSetOfADay(
//						appointmentSetWhereTheAppointmentWasDeleted =>
//						{
//							readModelRepository.RequestAppointmentSetOfADay(
//								appointmentSetWhereDeletedBeforeChange =>
//								{
//									UndoDividedReplacedEvents(addedEvent,
//															  deletedEvent,
//															  appointmentSetWhereTheAppointmentWasAdded,
//															  appointmentSetWhereTheAppointmentWasDeleted,
//															  appointmentSetWhereDeletedBeforeChange);
//
//									Application.Current.Dispatcher.Invoke(() =>
//									{
//										eventPointer = eventPointer.Previous.Previous;
//										CheckIfUndoAndRedoIsPossible();
//									});									
//								},
//								deletedEvent.AggregateId,
//								//eventPointer.Previous.Value.AggregateVersion-1,
//								errorCallback
//							);
//						},
//						deletedEvent.AggregateId,
//						//uint.MaxValue,
//						errorCallback
//					);
//				},
//				addedEvent.AggregateId,
//				//uint.MaxValue,
//				errorCallback
//			);			
//		}				
//
//		#region Undo-Actors
//
//		private void UndoRegularAddedEvent(AppointmentAdded addedEvent, FixedAppointmentSet currentAppointmentSet)
//		{			
//			commandBus.SendCommand(new DeleteAppointment(currentAppointmentSet.Identifier,
//														 currentAppointmentSet.AggregateVersion,
//														 currentUser.Id,
//														 addedEvent.PatientId,
//														 ActionTag.UndoAction,
//														 addedEvent.AppointmentId));
//		}
//
//		private void UndoRegularReplacedEvent(AppointmentReplaced replacedEvent, 
//											  FixedAppointmentSet currentAppointmentSet, 
//											  FixedAppointmentSet beforeChangeAppointmentSet)
//		{
//			var lastVersionOfTheAppointment = beforeChangeAppointmentSet.Appointments.First(
//													appointment => appointment.Id == replacedEvent.OriginalAppointmendId
//											  );
//
//			commandBus.SendCommand(new ReplaceAppointment(currentAppointmentSet.Identifier,
//														  currentAppointmentSet.Identifier,
//														  currentAppointmentSet.AggregateVersion,
//														  currentAppointmentSet.AggregateVersion,
//														  currentUser.Id,
//														  replacedEvent.PatientId,
//														  ActionTag.UndoAction,
//														  lastVersionOfTheAppointment.Description,
//														  lastVersionOfTheAppointment.Day,
//														  lastVersionOfTheAppointment.StartTime,
//														  lastVersionOfTheAppointment.EndTime,
//														  lastVersionOfTheAppointment.TherapyPlace.Id,
//														  lastVersionOfTheAppointment.Id,
//														  lastVersionOfTheAppointment.Day));
//		}
//
//		private void UndoRegularDeletedEvent(AppointmentDeleted deletedEvent, 
//											 FixedAppointmentSet currentAppointmentSet,
//											 FixedAppointmentSet beforeDeletedAppointmentSet)
//		{
//			var lastVersionOfTheAppointment = beforeDeletedAppointmentSet.Appointments.First(
//													appointment => appointment.Id == deletedEvent.RemovedAppointmentId
//											  );
//
//			commandBus.SendCommand(new AddAppointment(currentAppointmentSet.Identifier,
//													  currentAppointmentSet.AggregateVersion,
//													  currentUser.Id,
//													  ActionTag.UndoAction,
//													  deletedEvent.PatientId,
//													  lastVersionOfTheAppointment.Description,
//													  lastVersionOfTheAppointment.StartTime,
//													  lastVersionOfTheAppointment.EndTime,
//													  lastVersionOfTheAppointment.TherapyPlace.Id,
//													  lastVersionOfTheAppointment.Id));
//		}
//
//		private void UndoDividedReplacedEvents(AppointmentAdded addedEvent, AppointmentDeleted deletedEvent,
//											   FixedAppointmentSet appointmentSetWhereTheAppointmentWasAdded,
//											   FixedAppointmentSet appointmentSetWhereTheAppointmentWasDeleted,
//											   FixedAppointmentSet appointmentSetWhereDeletedBeforeChange)
//		{						
//			var lastVersionOfTheAppointment = appointmentSetWhereDeletedBeforeChange.Appointments.First(
//													appointment => appointment.Id == deletedEvent.RemovedAppointmentId
//											  );
//
//			commandBus.SendCommand(new DeleteAppointment(appointmentSetWhereTheAppointmentWasAdded.Identifier,
//														 appointmentSetWhereTheAppointmentWasAdded.AggregateVersion,
//														 currentUser.Id,
//														 addedEvent.PatientId,
//														 ActionTag.UndoDividedReplaceAction,
//														 addedEvent.AppointmentId));
//
//			commandBus.SendCommand(new AddAppointment(appointmentSetWhereTheAppointmentWasDeleted.Identifier,
//													  appointmentSetWhereTheAppointmentWasDeleted.AggregateVersion,
//													  currentUser.Id,
//													  ActionTag.UndoDividedReplaceAction,
//													  deletedEvent.PatientId,
//													  lastVersionOfTheAppointment.Description,
//													  lastVersionOfTheAppointment.StartTime,
//													  lastVersionOfTheAppointment.EndTime,
//													  lastVersionOfTheAppointment.TherapyPlace.Id,
//													  lastVersionOfTheAppointment.Id));			
//		}
//
//		#endregion
//		#endregion
//		#endregion
//
//		#region Redo
//
//		public void Redo ()
//		{
//			if (!RedoPossible)
//				throw new InvalidOperationException("redo not possible");
//
//			UndoPossible = false;
//			RedoPossible = false;
//
//			var domainEventToBeRestored = eventPointer.Next.Value;
//
//			switch (domainEventToBeRestored.ActionTag)
//			{
//				case ActionTag.RegularAction:
//				{
//					HandleNormalRedo(domainEventToBeRestored);
//					break;
//				}
//
//				case ActionTag.RegularDividedReplaceAction:
//				{
//					HandleDevidedRedo();
//					break;
//				}
//			}
//		}
//
//		#region Redo-Handler
//
//		private void HandleNormalRedo(DomainEvent domainEventToBeRestored)
//		{
//			readModelRepository.RequestAppointmentSetOfADay(
//				currentAppointmentSet =>
//				{
//
//					if (domainEventToBeRestored.GetType() == typeof(AppointmentAdded))
//					{						
//						RedoRegularAddedEvent((AppointmentAdded)domainEventToBeRestored, currentAppointmentSet);
//					}
//					else if (domainEventToBeRestored.GetType() == typeof(AppointmentReplaced))
//					{
//						RedoRegularReplacedEvent((AppointmentReplaced) domainEventToBeRestored, currentAppointmentSet);
//					}
//					else if (domainEventToBeRestored.GetType() == typeof(AppointmentDeleted))
//					{
//						RedoRegularDeletedEvent((AppointmentDeleted) domainEventToBeRestored, currentAppointmentSet);
//					}
//					else
//						throw new Exception("internal error");
//
//					Application.Current.Dispatcher.Invoke(() =>
//					{
//						eventPointer = eventPointer.Next;
//						CheckIfUndoAndRedoIsPossible();
//					});					
//				},
//				domainEventToBeRestored.AggregateId,
//				//uint.MaxValue,
//				errorCallback
//			);						
//		}
//
//		private void HandleDevidedRedo()
//		{
//			var currentEvent = eventPointer.Next.Value;
//			var nextEvent    = eventPointer.Next.Next.Value;
//
//			if (currentEvent.GetType() != typeof(AppointmentDeleted) || nextEvent.GetType() != typeof(AppointmentAdded))
//				throw new Exception("internal error");
//
//			var addedEvent   = (AppointmentAdded)   nextEvent;
//			var deletedEvent = (AppointmentDeleted) currentEvent;
//			
//
//			readModelRepository.RequestAppointmentSetOfADay(
//				appointmentSetWhereTheAppointmentIsToBeAdded =>
//				{
//
//					readModelRepository.RequestAppointmentSetOfADay(
//						appointmentSetWhereTheAppointmentIsToBeDeleted =>
//						{
//							RedoDevidedReplaceEvent(addedEvent, 
//													deletedEvent, 
//													appointmentSetWhereTheAppointmentIsToBeDeleted,
//													appointmentSetWhereTheAppointmentIsToBeAdded);
//
//							Application.Current.Dispatcher.Invoke(() =>
//							{
//								eventPointer = eventPointer.Next.Next;
//								CheckIfUndoAndRedoIsPossible();
//							});							
//						},
//						deletedEvent.AggregateId,
//						//uint.MaxValue,
//						errorCallback
//					);
//				},
//				addedEvent.AggregateId,
//				//uint.MaxValue,
//				errorCallback
//			);						
//		}
//
//		#region Redo-Actors
//
//		private void RedoRegularAddedEvent(AppointmentAdded addAppointmentEventToBeRestored, FixedAppointmentSet currentAppointmentSet)
//		{
//			commandBus.SendCommand(new AddAppointment(currentAppointmentSet.Identifier,
//													  currentAppointmentSet.AggregateVersion,
//													  currentUser.Id,
//													  ActionTag.RedoAction,
//													  addAppointmentEventToBeRestored.PatientId,
//													  addAppointmentEventToBeRestored.Description,
//													  addAppointmentEventToBeRestored.StartTime,
//													  addAppointmentEventToBeRestored.EndTime,
//													  addAppointmentEventToBeRestored.TherapyPlaceId,
//													  addAppointmentEventToBeRestored.AppointmentId));
//		}
//
//		private void RedoRegularReplacedEvent(AppointmentReplaced replacedEventToBeRestored, FixedAppointmentSet currentAppointmentSet)
//		{
//			commandBus.SendCommand(new ReplaceAppointment(currentAppointmentSet.Identifier,
//														  currentAppointmentSet.Identifier,
//														  currentAppointmentSet.AggregateVersion,
//														  currentAppointmentSet.AggregateVersion,
//														  currentUser.Id,
//														  replacedEventToBeRestored.PatientId,
//														  ActionTag.RedoAction,
//														  replacedEventToBeRestored.NewDescription,
//														  replacedEventToBeRestored.NewDate,
//														  replacedEventToBeRestored.NewStartTime,
//														  replacedEventToBeRestored.NewEndTime,
//														  replacedEventToBeRestored.NewTherapyPlaceId,
//														  replacedEventToBeRestored.OriginalAppointmendId,
//														  replacedEventToBeRestored.NewDate));
//		}
//
//		private void RedoRegularDeletedEvent(AppointmentDeleted deletedEventToBeRestored, FixedAppointmentSet currentAppointmentSet)
//		{
//			commandBus.SendCommand(new DeleteAppointment(currentAppointmentSet.Identifier,
//														 currentAppointmentSet.AggregateVersion,
//														 currentUser.Id,
//														 deletedEventToBeRestored.PatientId,
//														 ActionTag.RedoAction,
//														 deletedEventToBeRestored.RemovedAppointmentId));
//		}
//
//		private void RedoDevidedReplaceEvent(AppointmentAdded addedEvent, AppointmentDeleted deletedEvent, 
//											 FixedAppointmentSet appointmentSetWhereTheAppointmentIsToBeDeleted,
//											 FixedAppointmentSet appointmentSetWhereTheAppointmentIsToBeAdded)
//		{
//			commandBus.SendCommand(new DeleteAppointment(appointmentSetWhereTheAppointmentIsToBeDeleted.Identifier,
//														 appointmentSetWhereTheAppointmentIsToBeDeleted.AggregateVersion, 
//														 currentUser.Id, 
//														 deletedEvent.PatientId, 
//														 ActionTag.RedoDividedReplaceAction, 
//														 deletedEvent.RemovedAppointmentId));
//
//			commandBus.SendCommand(new AddAppointment(appointmentSetWhereTheAppointmentIsToBeAdded.Identifier,
//													  appointmentSetWhereTheAppointmentIsToBeAdded.AggregateVersion, 
//													  currentUser.Id, 
//													  ActionTag.RedoDividedReplaceAction, 
//													  addedEvent.PatientId, 
//													  addedEvent.Description, 
//													  addedEvent.StartTime, 
//													  addedEvent.EndTime, 
//													  addedEvent.TherapyPlaceId, 
//													  addedEvent.AppointmentId));
//		}
//
//		#endregion
//		#endregion
//		#endregion
//
//
//		public void RequestUndoActionMessage (Action<string> dataReceivedCallback, Action<string> localErrorCallback)
//		{
//			throw new NotImplementedException();
//		}
//		
//		public void RequestRedoActionMessage (Action<string> dataReceivedCallback, Action<string> localErrorCallback)
//		{
//			throw new NotImplementedException();
//		}
//
//		public string GetRedoActionMessage()
//	    {
////            if (!RedoPossible)
////                throw new InvalidOperationException("redo not possible");
////
////	        string result ="";
////
////            var domainEventToBeRestored = eventPointer.Next.Value;
////            var readModel = readModelRepository.GetAppointmentsOfADayReadModel(domainEventToBeRestored.AggregateId);
////
////            switch (domainEventToBeRestored.ActionTag)
////            {
////                case ActionTag.RegularAction:
////                {
////                    if (domainEventToBeRestored.GetType() == typeof(AppointmentAdded))
////                    {
////                        var addAppointmentEventToBeRestored = (AppointmentAdded)domainEventToBeRestored;
////
////                        result = RedoStringGenerator.ForAddedEvent(patientRepository.GetPatientById(addAppointmentEventToBeRestored.PatientId),
////                                                                    addAppointmentEventToBeRestored.CreateAppointmentData.Day,
////                                                                    addAppointmentEventToBeRestored.CreateAppointmentData.StartTime,
////                                                                    addAppointmentEventToBeRestored.CreateAppointmentData.EndTime);
////                           
////                    }
////                    else if (domainEventToBeRestored.GetType() == typeof(AppointmentReplaced))
////                    {
////                        var replacedEventToBeRestored = (AppointmentReplaced)domainEventToBeRestored;
////
////                        var currentVersionOfAppointment = readModel.Appointments.First(
////                            appointment => appointment.Id == replacedEventToBeRestored.OriginalAppointmendId
////                        );
////
////                        result = RedoStringGenerator.ForReplacedEvent(patientRepository.GetPatientById(replacedEventToBeRestored.PatientId), 
////																		replacedEventToBeRestored.NewDate,
////                                                                        currentVersionOfAppointment.StartTime,
////                                                                        currentVersionOfAppointment.EndTime,
////                                                                        currentVersionOfAppointment.TherapyPlace,
////                                                                        replacedEventToBeRestored.NewStartTime,
////                                                                        replacedEventToBeRestored.NewEndTime,
////                                                                        configuration.GetMedicalPracticeByIdAndVersion(readModel.Identifier.MedicalPracticeId,
////                                                                                                                    readModel.Identifier.PracticeVersion)
////                                                                                    .GetTherapyPlaceById(replacedEventToBeRestored.NewTherapyPlaceId)
////                                                                        );
////                    }
////                    else if (domainEventToBeRestored.GetType() == typeof(AppointmentDeleted))
////                    {
////                        var deletedEventToBeRestored = (AppointmentDeleted) domainEventToBeRestored;
////
////                        var currentVersionOfAppointment = readModel.Appointments.First(
////                            appointment => appointment.Id == deletedEventToBeRestored.RemovedAppointmentId
////                        );
////
////                        result = RedoStringGenerator.ForDeletedEvent(patientRepository.GetPatientById(deletedEventToBeRestored.PatientId),
////                                                                        currentVersionOfAppointment.Day,
////                                                                        currentVersionOfAppointment.StartTime,
////                                                                        currentVersionOfAppointment.EndTime);
////
////                    }
////                    else
////                        throw new Exception("internal error");
////                       
////                    break;
////                }
////
////                case ActionTag.RegularDividedReplaceAction:
////                {
////                    var currentEvent = eventPointer.Next.Value;
////                    var nextEvent = eventPointer.Next.Next.Value;
////
////                    if (currentEvent.GetType() != typeof(AppointmentDeleted) || nextEvent.GetType() != typeof(AppointmentAdded))
////                        throw new Exception("internal error");
////
////                    var addedEvent   = (AppointmentAdded)nextEvent;
////                    var deletedEvent = (AppointmentDeleted)currentEvent;
////
////                    var readModelWhereTheAppointmentIsToBeAdded   = readModelRepository.GetAppointmentsOfADayReadModel(addedEvent.AggregateId);
////                    var readModelWhereTheAppointmentIsToBeDeleted = readModelRepository.GetAppointmentsOfADayReadModel(deletedEvent.AggregateId);
////
////                    var currentVersionOfAppointment = readModelWhereTheAppointmentIsToBeDeleted.Appointments.First(
////                        appointment => appointment.Id == deletedEvent.RemovedAppointmentId
////                    );
////
////                    result = RedoStringGenerator.ForDividedReplacedEvent(patientRepository.GetPatientById(addedEvent.PatientId),
////                                                                            currentVersionOfAppointment.Day,
////                                                                            addedEvent.CreateAppointmentData.Day,
////                                                                            currentVersionOfAppointment.StartTime,
////                                                                            addedEvent.CreateAppointmentData.StartTime,
////                                                                            currentVersionOfAppointment.EndTime,
////                                                                            addedEvent.CreateAppointmentData.EndTime,
////                                                                            currentVersionOfAppointment.TherapyPlace,
////                                                                            configuration.GetMedicalPracticeByIdAndVersion(readModelWhereTheAppointmentIsToBeAdded.Identifier.MedicalPracticeId,
////                                                                                                                        readModelWhereTheAppointmentIsToBeAdded.Identifier.PracticeVersion)
////                                                                                        .GetTherapyPlaceById(addedEvent.CreateAppointmentData.TherapyPlaceId)
////                                                                            );
////
////                    readModelWhereTheAppointmentIsToBeAdded.Dispose();
////                    readModelWhereTheAppointmentIsToBeDeleted.Dispose();                        
////                    break;
////                }
////            }
////
////            readModel.Dispose();
////
////	        return result;
//
//			return "";
//	    }
//
//		public string GetUndoActionMessage()
//		{
////			if (!UndoPossible)
////                throw new InvalidOperationException("undo not possible");
////
////		    string result;
////
////            var domainEvent = eventPointer.Value;
////
////            switch (domainEvent.ActionTag)
////            {
////                case ActionTag.RegularAction:
////                {
////                    var readModel = readModelRepository.GetAppointmentsOfADayReadModel(domainEvent.AggregateId);
////
////                    if (domainEvent.GetType() == typeof(AppointmentAdded))
////                    {
////                        var addedEvent = (AppointmentAdded)domainEvent;
////
////                        result = UndoStringGenerator.ForAddedEvent(patientRepository.GetPatientById(addedEvent.PatientId),
////                                                                 readModel.Identifier.Date,
////                                                                 addedEvent.CreateAppointmentData.StartTime,
////                                                                 addedEvent.CreateAppointmentData.EndTime);                                                       
////                    }
////                    else if (domainEvent.GetType() == typeof(AppointmentReplaced))
////                    {
////                        var replacedEvent = (AppointmentReplaced)domainEvent;
////
////                        var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModel.Identifier,
////                                                                                              eventPointer.Value.AggregateVersion - 1);
////                                                
////                        var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
////                            appointment => appointment.Id == replacedEvent.OriginalAppointmendId
////                        );
////
////                        result = UndoStringGenerator.ForReplacedEvent(patientRepository.GetPatientById(replacedEvent.PatientId),
////                                                                      readModel.Identifier.Date,
////                                                                      replacedEvent.NewStartTime,
////                                                                      replacedEvent.NewEndTime,
////                                                                      configuration.GetMedicalPracticeByIdAndVersion(readModel.Identifier.MedicalPracticeId, 
////                                                                                                                     readModel.Identifier.PracticeVersion)
////																				   .GetTherapyPlaceById(replacedEvent.NewTherapyPlaceId),
////                                                                      lastVersionOfTheAppointment.StartTime,
////                                                                      lastVersionOfTheAppointment.EndTime,
////                                                                      lastVersionOfTheAppointment.TherapyPlace);                                                        
////                    }
////                    else if (domainEvent.GetType() == typeof(AppointmentDeleted))
////                    {
////                        var deletedEvent = (AppointmentDeleted)domainEvent;
////
////                        var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModel.Identifier,
////                                                                                                eventPointer.Value.AggregateVersion - 1);
////
////                        var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
////                            appointment => appointment.Id == deletedEvent.RemovedAppointmentId
////                        );
////
////                        result = UndoStringGenerator.ForDeletedEvent(patientRepository.GetPatientById(deletedEvent.PatientId),
////                                                                     readModel.Identifier.Date,
////                                                                     lastVersionOfTheAppointment.StartTime,
////                                                                     lastVersionOfTheAppointment.EndTime);
////                    }
////                    else
////                    {
////                        throw new Exception("internal error");
////                    }
////
////                    readModel.Dispose();                    
////                    break;
////                }
////                case ActionTag.RegularDividedReplaceAction:
////                {
////                    var currentEvent = eventPointer.Value;
////                    var lastEvent = eventPointer.Previous.Value;
////
////                    if (currentEvent.GetType() != typeof(AppointmentAdded) || lastEvent.GetType() != typeof(AppointmentDeleted))
////                        throw new Exception("internal error");
////
////                    var addedEvent = (AppointmentAdded)currentEvent;
////                    var deletedEvent = (AppointmentDeleted)lastEvent;
////
////                    var readModelWhereTheAppointmentWasAdded = readModelRepository.GetAppointmentsOfADayReadModel(addedEvent.AggregateId);
////                    var readModelWhereTheAppointmentWasDeleted = readModelRepository.GetAppointmentsOfADayReadModel(deletedEvent.AggregateId);
////
////                    var fixedAppointmentSet = readModelRepository.GetAppointmentSetOfADay(readModelWhereTheAppointmentWasDeleted.Identifier,
////                                                                                          eventPointer.Previous.Value.AggregateVersion - 1);
////
////                    var lastVersionOfTheAppointment = fixedAppointmentSet.Appointments.First(
////                            appointment => appointment.Id == deletedEvent.RemovedAppointmentId
////                    );
////
////                    result = UndoStringGenerator.ForDividedReplacedEvent(patientRepository.GetPatientById(deletedEvent.PatientId),
////                                                                         addedEvent.CreateAppointmentData.Day,       lastVersionOfTheAppointment.Day,
////                                                                         addedEvent.CreateAppointmentData.StartTime, lastVersionOfTheAppointment.StartTime,
////                                                                         addedEvent.CreateAppointmentData.EndTime,   lastVersionOfTheAppointment.EndTime,
////                                                                         configuration.GetMedicalPracticeByIdAndVersion(readModelWhereTheAppointmentWasAdded.Identifier.MedicalPracticeId,
////                                                                                                                     readModelWhereTheAppointmentWasAdded.Identifier.PracticeVersion)
////                                                                          .GetTherapyPlaceById(addedEvent.CreateAppointmentData.TherapyPlaceId),
////                                                                         lastVersionOfTheAppointment.TherapyPlace);
////                    readModelWhereTheAppointmentWasAdded.Dispose();
////                    readModelWhereTheAppointmentWasDeleted.Dispose();
////                    break;
////                }
////
////                default:
////                    throw new NotSupportedException();
////            }
////
////            return result;
//
//			return "";
//		}
//
//			   
//
//	    private void CheckIfUndoAndRedoIsPossible()
//	    {
//	        if (eventPointer != null)
//	        {
//	            UndoPossible = eventPointer.Previous != null;
//	            RedoPossible = eventPointer.Next != null;
//	        }
//	        else
//	        {
//	            UndoPossible = false;
//	            RedoPossible = false;
//	        }
//	    }
//
//	    private void RemoveAllFromEndTo(LinkedListNode<DomainEvent> node)
//	    {
//	        while (userTriggeredEvents.Last != node)
//	        {
//	            userTriggeredEvents.RemoveLast();
//
//	            if (userTriggeredEvents.Count == 0)
//	                throw new InvalidOperationException();
//	        }
//	    }
//
//
////	    public override void Process(AppointmentAdded domainEvent)
////	    {
////	        TryAddEvent(domainEvent);
////	    }
////
////	    public override void Process(AppointmentReplaced domainEvent)
////	    {
////	        TryAddEvent(domainEvent);
////	    }
////
////	    public override void Process(AppointmentDeleted domainEvent)
////	    {
////	        TryAddEvent(domainEvent);
////	    }	  
//
//	    public event PropertyChangedEventHandler PropertyChanged;
//	}
//
//}
