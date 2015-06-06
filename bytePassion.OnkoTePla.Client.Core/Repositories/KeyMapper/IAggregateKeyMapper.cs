using System;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.KeyMapper
{
	public interface IAggregateKeyMapper<in TKey>
	{
		Guid GetIdFor(TKey key);
		void AddKeyMap(TKey key, Guid id);

		bool HasKey(TKey key);
	}
}