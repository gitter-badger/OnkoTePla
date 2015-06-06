using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.KeyMapper
{
	public class AggregateKeyMapper<TKey> : IPersistablility, IAggregateKeyMapper<TKey>
	{
		private readonly IPersistenceService<IDictionary<TKey, Guid>> persistenceService;
		private IDictionary<TKey, Guid> keyMap;

		public AggregateKeyMapper(IPersistenceService<IDictionary<TKey, Guid>> persistenceService)
		{
			this.persistenceService = persistenceService;
		}

		public Guid GetIdFor(TKey key)
		{
			if (keyMap.ContainsKey(key))
				return keyMap[key];

			throw new ArgumentException("no id found for this key");
		}

		public void AddKeyMap(TKey key, Guid id)
		{
			if (keyMap.ContainsKey(key))
				throw new ArgumentException("this key already exists");

			keyMap.Add(key, id);
		}

		public bool HasKey(TKey key)
		{
			return keyMap.ContainsKey(key);
		}

		public void PersistRepository()
		{
			persistenceService.Persist(keyMap);
		}

		public void LoadRepository()
		{
			keyMap = persistenceService.Load();
		}
	}
}