using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Data;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class DataCenterBuilder : IDataCenterBuilder
	{
		private readonly IConfigurationReadRepository readConfig;
		private readonly IConfigurationWriteRepository writeConfig;

		public DataCenterBuilder(IConfigurationReadRepository readConfig, 
								 IConfigurationWriteRepository writeConfig)
		{
			this.readConfig = readConfig;
			this.writeConfig = writeConfig;
		}

		public IDataCenter Build()
		{
			return new DataCenter(readConfig, writeConfig);
		}
	}
}
