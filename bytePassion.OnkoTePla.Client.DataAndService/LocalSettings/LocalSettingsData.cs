namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	internal class LocalSettingsData
	{
		public static LocalSettingsData CreateDefaultSettings()
		{
			return new LocalSettingsData(false);
		}

		public LocalSettingsData(bool isAutoConnectionEnabled)
		{
			IsAutoConnectionEnabled = isAutoConnectionEnabled;
		}

		public bool IsAutoConnectionEnabled { get; }
	}
}