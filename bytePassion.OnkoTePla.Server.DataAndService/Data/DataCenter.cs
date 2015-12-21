using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	internal class DataCenter : IDataCenter
	{
		public Address ServerAddress { get; }
	}

	public interface IDataCenter
	{
		Address ServerAddress { get; }
	}
}
