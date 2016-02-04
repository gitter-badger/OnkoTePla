namespace bytePassion.OnkoTePla.Resources
{
	public static class GlobalConstants
    {		
		public static class TcpIpPort
		{						 
			public const uint Heartbeat = 6656;
			public const uint Request   = 6657;
			public const uint EventBus  = 6658;

			public const uint Free2     = 6659;
			public const uint Free3     = 6660;
			public const uint Free4     = 6661;
			public const uint Free5     = 6662;
		}
		
		public const uint StandardSendingTimeout			 =  2000;    // unit is milliseconds
		public const uint HeartbeatIntverval				 =  5000;    //			"
		public const uint ClientWaitTimeForHeartbeat		 = 10000;	 //			"
		public const uint ServerWaitTimeForHeartbeatResponse =  2000;    //			"

		public const string PatientPersistenceFile          = @"..\..\..\AppData\patients.xml";		
		public const string ConfigPersistenceFile           = @"..\..\..\AppData\config.xml";
		public const string EventHistoryPersistenceFile     = @"..\..\..\AppData\eventHistory.xml";
		public const string LocalSettingsPersistenceFile    = @"..\..\..\AppData\settings.xml";

		public const string PatientJsonPersistenceFile      = @"..\..\..\AppData\patients.json";
		public const string ConfigJsonPersistenceFile       = @"..\..\..\AppData\config.json";		
		public const string EventHistoryJsonPersistenceFile = @"..\..\..\AppData\eventHistory.json";

        public const string EventHistoryBasePath = @"..\..\..\AppData\EventHistory";
    }
}
