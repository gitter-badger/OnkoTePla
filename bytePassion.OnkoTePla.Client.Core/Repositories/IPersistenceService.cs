
namespace bytePassion.OnkoTePla.Client.Core.Repositories
{
	public interface IPersistenceService<T>
	{
		void Persist(T data);
		T Load();
	}
}
