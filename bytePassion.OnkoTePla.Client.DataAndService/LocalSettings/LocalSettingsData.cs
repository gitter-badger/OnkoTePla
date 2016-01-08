using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	internal class LocalSettingsData
	{
		public static LocalSettingsData CreateDefaultSettings()
		{
			return new LocalSettingsData(false, 
										 new IpV4AddressIdentifier(127, 0, 0, 1),
										 new IpV4AddressIdentifier(127, 0, 0, 1));
		}

		public LocalSettingsData(bool isAutoConnectionEnabled, 
								 AddressIdentifier autoConnectionClientAddress, 
								 AddressIdentifier autoConnectionServerAddress)
		{
			IsAutoConnectionEnabled = isAutoConnectionEnabled;
			AutoConnectionClientAddress = autoConnectionClientAddress;
			AutoConnectionServerAddress = autoConnectionServerAddress;
		}

		public bool              IsAutoConnectionEnabled     { get; }
		public AddressIdentifier AutoConnectionClientAddress { get; }
		public AddressIdentifier AutoConnectionServerAddress { get; }
	}
}