using bytePassion.Lib.Types.Repository;

namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	internal interface ILocalSettingsRepository : IPersistable
	{
		bool IsAutoConnectionEnabled { get; set; }
	}
}