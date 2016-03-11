namespace bytePassion.OnkoTePla.Resources
{
	public static class GlobalConstants
    {		
		public static class TcpIpPort
		{						 
			public const uint Heartbeat    = 6656;
			public const uint Request      = 6657;
			public const uint Notification = 6658;			
		}

		public static readonly char[] ForbiddenCharacters = {'|',';',',',':','.','#',
															 '(','[','{','}',']',')' };

		public const uint StandardSendingTimeout			 =  2000;    // unit is milliseconds
		public const uint HeartbeatIntverval				 =  5000;    //			"
		public const uint ClientWaitTimeForHeartbeat		 = 10000;	 //			"
		public const uint ServerWaitTimeForHeartbeatResponse =  2000;    //			"

		public const string PatientPersistenceFile          = @"..\..\..\AppData\patients.xml";		
		public const string ConfigPersistenceFile           = @"..\..\..\AppData\config.xml";		
		public const string LocalSettingsPersistenceFile    = @"..\..\..\AppData\settings.xml";
		public const string MetaDataPersistanceFile         = @"..\..\..\AppData\metaData.xml";
		
		public const string EventHistoryBasePath            = @"..\..\..\AppData\EventHistory";		
    }
}
