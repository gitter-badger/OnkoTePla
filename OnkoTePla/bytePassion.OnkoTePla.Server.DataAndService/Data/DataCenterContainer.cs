using System;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	public class DataCenterContainer
	{
		public event Action<DataCenterContainer, IDataCenter> DataCenterAvailable;

		private IDataCenter dataCenter;

		public IDataCenter DataCenter
		{
			get { return dataCenter; }
			set
			{
				dataCenter = value;
				DataCenterAvailable?.Invoke(this, dataCenter);
			}
		}
	}
}
