using System;
using System.Collections.Generic;
using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Contracts.Config;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.Core.Readmodels
{
	public class SessionAndUserSpecificEventHistory : ReadModelBase,
													  IUndoRedo													  
	{
		private readonly ICommandBus commandBus;
		private readonly IReadModelRepository readModelRepository;
		private readonly User currentUser;		
		private readonly uint maximalSavedVersions;
		private readonly LinkedList<DomainEvent> userTriggeredEvents;

		private LinkedListNode<DomainEvent> lastTriggeredEventPointer;

		private bool undoPossible;
		private bool redoPossible;

		public SessionAndUserSpecificEventHistory(IEventBus eventBus,
												  ICommandBus commandBus,
												  IReadModelRepository readModelRepository,
												  User currentUser,
												  uint maximalSavedVersions) 
			: base(eventBus)
		{
			this.commandBus = commandBus;
			this.readModelRepository = readModelRepository;
			this.currentUser = currentUser;
			

			this.maximalSavedVersions = maximalSavedVersions;
			userTriggeredEvents = new LinkedList<DomainEvent>();
			lastTriggeredEventPointer = null;

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

				RemoveAllFromEndTo(lastTriggeredEventPointer);
				userTriggeredEvents.AddLast(newVersionNode);
				lastTriggeredEventPointer = newVersionNode;

				if (userTriggeredEvents.Count == maximalSavedVersions + 1)
					userTriggeredEvents.RemoveFirst();
			}
		}


		public void Undo ()
		{			
			if (UndoPossible)
			{
				var domainEvent = lastTriggeredEventPointer.Value;
                switch (domainEvent.ActionTag)
				{
					case ActionTag.RegularAction:
					{
						
						if (domainEvent.GetType() == typeof(AppointmentAdded))
						{
							var addedEvent = (AppointmentAdded) domainEvent;

							var readModel = readModelRepository.GetAppointmentsOfADayReadModel(addedEvent.AggregateId);
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

							
						}
						else if (domainEvent.GetType() == typeof(AppointmentDeleted))
						{
							var deletedEvent = (AppointmentDeleted) domainEvent;
							

						}
						else
						{
							throw new Exception("internal error");
						}

						break;
					}
					case ActionTag.RegularDividedReplaceAction:
					{
						break;
					}
				}

				lastTriggeredEventPointer = lastTriggeredEventPointer.Previous;

				// TODO !!!!

				CheckIfUndoAndRedoIsPossible();
			}
			else
				throw new InvalidOperationException("undo not possible");
			
		}

		public void Redo ()
		{			
			if (RedoPossible)
			{
				lastTriggeredEventPointer = lastTriggeredEventPointer.Next;

				// TODO !!!!

				CheckIfUndoAndRedoIsPossible();
			}
			else
				throw new InvalidOperationException("redo not possible");			
		}

		private void CheckIfUndoAndRedoIsPossible ()
		{
			if (lastTriggeredEventPointer != null)
			{
				UndoPossible = lastTriggeredEventPointer.Previous != null;
				RedoPossible = lastTriggeredEventPointer.Next != null;
			}
			else
			{
				UndoPossible = false;
				RedoPossible = false;
			}
		}

		private void RemoveAllFromEndTo (LinkedListNode<DomainEvent> node)
		{
			while (userTriggeredEvents.Last != node)
			{
				userTriggeredEvents.RemoveLast();

				if (userTriggeredEvents.Count == 0)
					throw new InvalidOperationException();
			}
		}


		public override void Process(AppointmentAdded    domainEvent) { TryAddEvent(domainEvent); }
		public override void Process(AppointmentReplaced domainEvent) { TryAddEvent(domainEvent); }
		public override void Process(AppointmentDeleted  domainEvent) { TryAddEvent(domainEvent); }

				
		public override event EventHandler<AppointmentChangedEventArgs> AppointmentChanged;

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
