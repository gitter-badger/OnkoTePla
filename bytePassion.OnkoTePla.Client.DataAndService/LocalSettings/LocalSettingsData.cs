using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	internal class LocalSettingsData
	{
		public static LocalSettingsData CreateDefaultSettings()
		{
			return new LocalSettingsData(false, 
										 new InProcIdentifier("noAddressNeeded"));
		}

		public LocalSettingsData(bool isAutoConnectionEnabled, 
								 AddressIdentifier autoConnectionAddress)
		{
			IsAutoConnectionEnabled = isAutoConnectionEnabled;
			AutoConnectionAddress = autoConnectionAddress;
		}

		public bool              IsAutoConnectionEnabled { get; }
		public AddressIdentifier AutoConnectionAddress   { get; }
	}
}