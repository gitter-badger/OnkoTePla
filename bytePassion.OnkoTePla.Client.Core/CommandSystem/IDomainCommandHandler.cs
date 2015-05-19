namespace bytePassion.OnkoTePla.Client.Core.CommandSystem
{

	public interface IDomainCommandHandler<TCommand>
	{
		void Execute(TCommand command);
	}
}