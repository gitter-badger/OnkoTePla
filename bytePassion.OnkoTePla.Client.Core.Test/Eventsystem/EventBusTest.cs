using System;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using Xunit;

namespace bytePassion.OnkoTePla.Client.Core.Test.Eventsystem
{
	public class EventBusTest
	{

		private static AppointmentAdded GetAppointmentAddedDummy()
		{
			return new AppointmentAdded(new AggregateIdentifier(Date.Dummy, new Guid()), 0, new Guid(), null,ActionTag.RegularAction, new CreateAppointmentData(new Guid(),null, new Time(),new Time(),new Date(), new Guid(),new Guid()));
		}

		private static AppointmentDeleted GetAppointmentRemovedDummy()
		{
			return new AppointmentDeleted(new AggregateIdentifier(Date.Dummy, new Guid()), 0, new Guid(), new Guid(), new Tuple<Date, Time>(Date.Dummy, Time.Dummy),ActionTag.RegularAction, new Guid());
		}

		private class TestSingleEventHandler : IDomainEventHandler<AppointmentAdded>
		{

			public TestSingleEventHandler()
			{
				HandledEvent = false;
			}

			public void Process(AppointmentAdded domainEvent)
			{
				HandledEvent = true;
			}

			public bool HandledEvent { private set; get; }
		}
		
		[Fact]
		public void SingleSubscribscriptionTest()
		{
			IHandlerCollection<DomainEvent> eventHandlerCollection = new MultiHandlerCollection<DomainEvent>();
			IMessageBus<DomainEvent> eventBus = new LocalMessageBus<DomainEvent>(eventHandlerCollection);
			var testEventHandler = new TestSingleEventHandler();

			eventBus.RegisterMessageHandler(testEventHandler);
			eventBus.Send(GetAppointmentAddedDummy());

			Assert.True(testEventHandler.HandledEvent);
		}
		
		
		private class TestDoubleEventHandler : IDomainEventHandler<AppointmentAdded>,
											   IDomainEventHandler<AppointmentDeleted>
		{
			public TestDoubleEventHandler()
			{
				HandleAddedEvent = false;
				HandleRemovedEvent = false;
			}

			public void Process (AppointmentAdded domainEvent)
			{
				HandleAddedEvent = true;
			}

			public void Process (AppointmentDeleted domainEvent)
			{
				HandleRemovedEvent = true;
			}

			public bool HandleAddedEvent { private set; get; }
			public bool HandleRemovedEvent { private set; get; }
		}

		[Fact]
		public void DoubleSubscriptionTest()
		{
			IHandlerCollection<DomainEvent> eventHandlerCollection = new MultiHandlerCollection<DomainEvent>();
			IMessageBus<DomainEvent> eventBus = new LocalMessageBus<DomainEvent>(eventHandlerCollection);

			var testEventHandler = new TestDoubleEventHandler();

			eventBus.RegisterMessageHandler<AppointmentAdded>(testEventHandler);
			eventBus.RegisterMessageHandler<AppointmentDeleted>(testEventHandler);

			eventBus.Send(GetAppointmentAddedDummy());

			Assert.True(testEventHandler.HandleAddedEvent);
			Assert.False(testEventHandler.HandleRemovedEvent);

			eventBus.Send(GetAppointmentRemovedDummy());

			Assert.True(testEventHandler.HandleAddedEvent);
			Assert.True(testEventHandler.HandleRemovedEvent);
		}

		[Fact]
		public void SubscriptionOfTwoHandlerTest()
		{
			IHandlerCollection<DomainEvent> eventHandlerCollection = new MultiHandlerCollection<DomainEvent>();
			IMessageBus<DomainEvent> eventBus = new LocalMessageBus<DomainEvent>(eventHandlerCollection);

			var testEventHandler1 = new TestSingleEventHandler();
			var testEventHandler2 = new TestSingleEventHandler();

			eventBus.RegisterMessageHandler(testEventHandler1);
			eventBus.RegisterMessageHandler(testEventHandler2);

			eventBus.Send(GetAppointmentAddedDummy());
			Assert.True(testEventHandler1.HandledEvent);
			Assert.True(testEventHandler2.HandledEvent);
		}

		private class TestAnotherSingleEventHandler : IDomainEventHandler<AppointmentDeleted>
		{

			public TestAnotherSingleEventHandler ()
			{
				HandledEvent = false;
			}

			public void Process (AppointmentDeleted domainEvent)
			{
				HandledEvent = true;
			}

			public bool HandledEvent { private set; get; }
		}

		[Fact]
		public void SubscriptionOfTwoDistinctHandlerTest ()
		{
			IHandlerCollection<DomainEvent> eventHandlerCollection = new MultiHandlerCollection<DomainEvent>();
			IMessageBus<DomainEvent> eventBus = new LocalMessageBus<DomainEvent>(eventHandlerCollection);

			var testEventHandler1 = new TestSingleEventHandler();
			var testEventHandler2 = new TestAnotherSingleEventHandler();

			eventBus.RegisterMessageHandler(testEventHandler1);
			eventBus.RegisterMessageHandler(testEventHandler2);

			eventBus.Send(GetAppointmentAddedDummy());

			Assert.True(testEventHandler1.HandledEvent);
			Assert.False(testEventHandler2.HandledEvent);

			eventBus.Send(GetAppointmentRemovedDummy());

			Assert.True(testEventHandler1.HandledEvent);
			Assert.True(testEventHandler2.HandledEvent);
		}
	}
}
