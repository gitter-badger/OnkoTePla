

namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.Base
{
	public interface IDomainEventHandler<in TEvent>
	{
		void Handle(TEvent domainEvent);		
	}
}