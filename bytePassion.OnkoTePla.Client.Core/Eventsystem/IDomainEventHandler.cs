

namespace bytePassion.OnkoTePla.Client.Core.Eventsystem
{
	public interface IDomainEventHandler<in TEvent>
	{
		void Handle(TEvent domainEvent);		
	}
}