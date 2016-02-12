//using bytePassion.Lib.Communication.MessageBus;
//
//
//namespace bytePassion.OnkoTePla.Core.Eventsystem
//{
//    public class EventBus : IEventBus
//	{
//		private readonly IMessageBus<DomainEvent> eventMessageBus;
//
//		public EventBus(IMessageBus<DomainEvent> eventMessageBus)
//		{
//			this.eventMessageBus = eventMessageBus;
//		}
//
//		public void RegisterEventHandler<TDomainEvent>(IMessageHandler<TDomainEvent> eventHandler) 
//			where TDomainEvent : DomainEvent
//		{
//			eventMessageBus.RegisterMessageHandler(eventHandler);
//		}
//
//		public void DeregisterEventHander<TDomainEvent>(IMessageHandler<TDomainEvent> eventHandler) 
//			where TDomainEvent : DomainEvent
//		{
//			eventMessageBus.DeregisterMessageHander(eventHandler);
//		}
//
//		public void PublishEvent<TDomainEvent>(TDomainEvent @event) 
//			where TDomainEvent : DomainEvent
//		{
//			eventMessageBus.Send(@event);
//		}
//	}
//}
