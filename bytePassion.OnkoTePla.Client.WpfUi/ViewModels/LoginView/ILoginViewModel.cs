using System.Collections.ObjectModel;
using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView
{
	internal interface ILoginViewModel : IViewModel
    {
        ICommand Login      { get; }
		ICommand Connect    { get; }
		ICommand Disconnect { get; }

		ObservableCollection<string> AvailableUsers { get; }

		string SelectedUserName { get; set; }

		string Password { set; }
		string ServerAddress { get; set; }

		bool AutoConnectOnNextStart { get; set; }
    }
}