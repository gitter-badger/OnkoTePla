using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;

namespace bytePassion.OnkoTePla.Client.DataAndService.Factorys
{
	public interface ISessionBuilder
	{
		ISession Build();
	}
}