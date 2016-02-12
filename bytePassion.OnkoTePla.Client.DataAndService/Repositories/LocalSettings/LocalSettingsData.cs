using System;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.LocalSettings
{
	public class LocalSettingsData
	{
		public static LocalSettingsData CreateDefaultSettings()
		{
			return new LocalSettingsData(false, 
										 new IpV4AddressIdentifier(127, 0, 0, 1),
										 new IpV4AddressIdentifier(127, 0, 0, 1),
										 Guid.Empty);
		}

		public LocalSettingsData(bool isAutoConnectionEnabled, 
								 AddressIdentifier autoConnectionClientAddress, 
								 AddressIdentifier autoConnectionServerAddress, Guid lastUsedMedicalPracticeId)
		{
			IsAutoConnectionEnabled = isAutoConnectionEnabled;
			AutoConnectionClientAddress = autoConnectionClientAddress;
			AutoConnectionServerAddress = autoConnectionServerAddress;
			LastUsedMedicalPracticeId = lastUsedMedicalPracticeId;
		}

		public bool              IsAutoConnectionEnabled     { get; }
		public AddressIdentifier AutoConnectionClientAddress { get; }
		public AddressIdentifier AutoConnectionServerAddress { get; }
		public Guid              LastUsedMedicalPracticeId   { get; }
	}
}