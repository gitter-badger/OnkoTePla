using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace bytePassion.Lib.Communication.MessageBus.HandlerCollection
{

	public class MultiHandlerCollection<TMessageBase> : IHandlerCollection<TMessageBase>
	{			
		private readonly IDictionary<Type, IList> eventHandlerLists;
		
		public MultiHandlerCollection ()
		{
			eventHandlerLists = new Dictionary<Type, IList>();
		}

		public void Add<TMessage> (IMessageHandler<TMessage> newEventHandler) where TMessage : TMessageBase
		{
			if (!eventHandlerLists.ContainsKey(typeof(TMessage)))
				eventHandlerLists.Add(typeof(TMessage), new ArrayList());

			eventHandlerLists[typeof(TMessage)].Add(newEventHandler);
		}

		public void Remove<TMessage> (IMessageHandler<TMessage> eventHandlerToRemove) where TMessage : TMessageBase
		{
			if (!eventHandlerLists.ContainsKey(typeof(TMessage)))
				return;

			eventHandlerLists[typeof(TMessage)].Remove(eventHandlerToRemove);

			if (eventHandlerLists[typeof(TMessage)].Count == 0)
				eventHandlerLists.Remove(typeof(TMessage));
		}

		public IEnumerable<IMessageHandler<TMessage>> GetMessageHandler<TMessage> () where TMessage : TMessageBase
		{
			if (!eventHandlerLists.ContainsKey(typeof(TMessage)))
				return null;

			return eventHandlerLists[typeof(TMessage)]
					.Cast<IMessageHandler<TMessage>>()
					.ToList();
		}
	}

}