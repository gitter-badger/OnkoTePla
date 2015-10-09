using System;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.Test
{
	public class MessageBusTest
	{
		private static AppointmentAdded GetAppointmentAddedDummy ()
		{
			return new AppointmentAdded(new AggregateIdentifier(Date.Dummy, new Guid()), 0, new Guid(), null, ActionTag.RegularAction, new CreateAppointmentData(new Guid(), null, new Time(), new Time(), new Date(), new Guid(), new Guid()));
		}		

		private class TestSingleEventHandler : IDomainEventHandler<AppointmentAdded>
		{

			public TestSingleEventHandler ()
			{
				HandledEvent = false;
			}

			public void Process (AppointmentAdded domainEvent)
			{
				HandledEvent = true;
			}

			public bool HandledEvent { private set; get; }
		}


		public void NoSubscribscriptionTest ()
		{
			IHandlerCollection<DomainEvent> eventHandlerCollection = new MultiHandlerCollection<DomainEvent>();
			IMessageBus<DomainEvent> eventBus = new LocalMessageBus<DomainEvent>(eventHandlerCollection);
						
			eventBus.Send(GetAppointmentAddedDummy());			
		}

		public void SingleSubscribscriptionTest ()
		{
			IHandlerCollection<DomainEvent> eventHandlerCollection = new MultiHandlerCollection<DomainEvent>();
			IMessageBus<DomainEvent> eventBus = new LocalMessageBus<DomainEvent>(eventHandlerCollection);
			var testEventHandler = new TestSingleEventHandler();

			eventBus.RegisterMessageHandler(testEventHandler);
			eventBus.Send(GetAppointmentAddedDummy());

			if (!testEventHandler.HandledEvent)
			{
				throw new ArgumentException();
			}
		}
	}
}
