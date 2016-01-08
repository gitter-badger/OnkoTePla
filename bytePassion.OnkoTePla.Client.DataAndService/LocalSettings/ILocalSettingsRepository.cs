using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.Types.Repository;

namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	internal interface ILocalSettingsRepository : IPersistable
	{
		bool              IsAutoConnectionEnabled     { get; set; }
		AddressIdentifier AutoConnectionClientAddress { get; set; }
		AddressIdentifier AutoConnectionServerAddress { get; set; }
	}
}