﻿using bytePassion.OnkoTePla.Server.DataAndService.Data;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class DataCenterBuilder : IDataCenterBuilder
	{
		public IDataCenter Build()
		{
			return new DataCenter();
		}
	}
}