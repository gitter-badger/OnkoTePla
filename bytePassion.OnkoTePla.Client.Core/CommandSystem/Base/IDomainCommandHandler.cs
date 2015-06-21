namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.Base
{

	public interface IDomainCommandHandler<TCommand>
	{
		void Execute(TCommand command);
	}
}