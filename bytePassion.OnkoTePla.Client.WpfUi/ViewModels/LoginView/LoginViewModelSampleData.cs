using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView
{
	internal class LoginViewModelSampleData : ILoginViewModel
    {
	    public LoginViewModelSampleData()
	    {
		    AvailableUsers = new ObservableCollection<string>
		    {
			    "User1",
				"User2"
		    };

			ClientIpAddresses = new ObservableCollection<string>
			{
				"192.168.128.14",
				"192.168.128.13"
			};

		    SelectedUserName = "User2";
		    ServerAddress = "192.168.128.12";
			ClientAddress = "192.168.128.13";

			AutoConnectOnNextStart = true;
	    }

	    public ICommand Login        => null;
	    public ICommand Connect      => null;
		public ICommand DebugConnect => null;
		public ICommand Disconnect   => null;

	    public ObservableCollection<string> AvailableUsers { get; }
		public ObservableCollection<string> ClientIpAddresses { get; }

		public string SelectedUserName  { get; set;  }
	    public string Password          {      set {}}
	    public string ServerAddress     { get; set;  }
		public string ClientAddress     { get; set;  }

		public bool AutoConnectOnNextStart { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}