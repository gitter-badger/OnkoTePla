﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage.Helper;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage
{
	internal class ConnectionsPageViewModelSampleData : IConnectionsPageViewModel
    {
	    public ConnectionsPageViewModelSampleData()
	    {
		    IsConnectionActive   = true;
		    IsActivationPossible = true;
			
			AvailableIpAddresses = new ObservableCollection<string>
			{
				"192.168.127.23",
				"10.72.30.5"
			};

		    ActiveConnection  = AvailableIpAddresses.First();
		    SelectedIpAddress = AvailableIpAddresses.First();

			ConnectedClients = new ObservableCollection<ConnectedClientDisplayData>
			{
				new ConnectedClientDisplayData(new ConnectionSessionId(Guid.NewGuid()).ToString(),
											   new Time(10,23,45).ToString(),
											   "192.168.127.11"),

				new ConnectedClientDisplayData(new ConnectionSessionId(Guid.NewGuid()).ToString(),
											   new Time(9,43,45).ToString(),
											   "192.168.127.12"),

				new ConnectedClientDisplayData(new ConnectionSessionId(Guid.NewGuid()).ToString(),
											   new Time(12,53,45).ToString(),
											   "192.168.127.13")
			};
	    }

	    public ICommand UpdateAvailableAddresses => null;

	    public bool IsConnectionActive   { get; set; }
		public bool IsActivationPossible { get; }

		public string SelectedIpAddress { get; set; }
		public string ActiveConnection  { get; }

		public ObservableCollection<string> AvailableIpAddresses { get; }
		public ObservableCollection<ConnectedClientDisplayData> ConnectedClients { get; }

		public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;	    
    }
}