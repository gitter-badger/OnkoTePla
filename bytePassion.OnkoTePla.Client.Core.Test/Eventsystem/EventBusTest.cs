using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using Xunit;

namespace bytePassion.OnkoTePla.Client.Core.Test.Eventsystem
{
	public class EventBusTest
	{

		private static AppointmentAdded GetAppointmentAddedDummy()
		{
			return new AppointmentAdded(new AggregateIdentifier(Date.Dummy, new Guid()), 0, new Guid(), null, new CreateAppointmentData(new Guid(),null, new Time(),new Time(),new Date(), new Guid(),new Guid()));
		}

		private class TestSingleEventHandler : IDomainEventHandler<AppointmentAdded>
		{

			public TestSingleEventHandler()
			{
				HandledEvent = false;
			}

			public void Handle(AppointmentAdded domainEvent)
			{
				HandledEvent = true;
			}

			public bool HandledEvent { private set; get; }
		}
		
		[Fact]
		public void SingleSubscribscriptionTest()
		{
			IEventBus eventBus = new EventBus();
			var testEventHandler = new TestSingleEventHandler();

			eventBus.RegisterEventHandler(testEventHandler);
			eventBus.Publish(GetAppointmentAddedDummy());

			Assert.True(testEventHandler.HandledEvent);
		}
		
		
		private class TestDoubleEventHandler : IDomainEventHandler<AppointmentAdded>,
											   IDomainEventHandler<AppointmentRemoved>
		{
			public TestDoubleEventHandler()
			{
				HandleAddedEvent = false;
				HandleRemovedEvent = false;
			}
			
			public void Handle(AppointmentAdded domainEvent)
			{
				HandleAddedEvent = true;
			}

			public void Handle(AppointmentRemoved domainEvent)
			{
				HandleRemovedEvent = true;
			}

			public bool HandleAddedEvent { private set; get; }
			public bool HandleRemovedEvent { private set; get; }
		}

		[Fact]
		public void DoubleSubscriptionTest()
		{
			IEventBus eventBus = new EventBus();
			var testEventHandler = new TestDoubleEventHandler();

			eventBus.RegisterEventHandler<AppointmentAdded>(testEventHandler);
			eventBus.RegisterEventHandler<AppointmentRemoved>(testEventHandler);

			eventBus.Publish(GetAppointmentAddedDummy());

			Assert.True(testEventHandler.HandleAddedEvent);
			Assert.False(testEventHandler.HandleRemovedEvent);

			eventBus.Publish(GetAppointmentAddedDummy());

			Assert.True(testEventHandler.HandleAddedEvent);
			Assert.True(testEventHandler.HandleRemovedEvent);
		}

		[Fact]
		public void SubscriptionOfTwoHandlerTest()
		{
			IEventBus eventBus = new EventBus();
			var testEventHandler1 = new TestSingleEventHandler();
			var testEventHandler2 = new TestSingleEventHandler();

			eventBus.RegisterEventHandler(testEventHandler1);
			eventBus.RegisterEventHandler(testEventHandler2);

			eventBus.Publish(GetAppointmentAddedDummy());
			Assert.True(testEventHandler1.HandledEvent);
			Assert.True(testEventHandler2.HandledEvent);
		}

		private class TestAnotherSingleEventHandler : IDomainEventHandler<AppointmentRemoved>
		{

			public TestAnotherSingleEventHandler ()
			{
				HandledEvent = false;
			}		
			
			public void Handle(AppointmentRemoved domainEvent)
			{
				HandledEvent = true;
			}

			public bool HandledEvent { private set; get; }
		}

		[Fact]
		public void SubscriptionOfTwoDistinctHandlerTest ()
		{
			IEventBus eventBus = new EventBus();
			var testEventHandler1 = new TestSingleEventHandler();
			var testEventHandler2 = new TestAnotherSingleEventHandler();

			eventBus.RegisterEventHandler(testEventHandler1);
			eventBus.RegisterEventHandler(testEventHandler2);

			eventBus.Publish(GetAppointmentAddedDummy());

			Assert.True(testEventHandler1.HandledEvent);
			Assert.False(testEventHandler2.HandledEvent);

			eventBus.Publish(GetAppointmentAddedDummy());

			Assert.True(testEventHandler1.HandledEvent);
			Assert.True(testEventHandler2.HandledEvent);
		}
	}
}
