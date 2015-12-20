using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Utils;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WpfUi.Enums;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar
{
	internal class ActionBarViewModel : ViewModel, IActionBarViewModel
    {
        private readonly IViewModelCommunication viewModelCommunication;
        private readonly IWindowBuilder<Views.AboutDialog> dialogBuilder;

        public ActionBarViewModel(IConnectionStatusViewModel connectionStatusViewModel,
                                  IViewModelCommunication viewModelCommunication,
                                  IWindowBuilder<Views.AboutDialog> dialogBuilder)
        {
            this.viewModelCommunication = viewModelCommunication;
            this.dialogBuilder = dialogBuilder;
            ConnectionStatusViewModel = connectionStatusViewModel;

            ShowOverview = new Command(() => viewModelCommunication.Send(new ShowPage(MainPage.Overview)));
            ShowSearch   = new Command(() => viewModelCommunication.Send(new ShowPage(MainPage.Search)));
            ShowOptions  = new Command(() => viewModelCommunication.Send(new ShowPage(MainPage.Options)));

            ShowAbout = new Command(ShowAboutDialog);
			Logout = new Command(DoLogOut);
        }

	    private static void DoLogOut()
	    {
			//			var host = Dns.GetHostEntry(Dns.GetHostName());
			//			foreach (var ip in host.AddressList)
			//			{
			//				if (ip.AddressFamily == AddressFamily.InterNetwork)
			//				{
			//					MessageBox.Show(ip.ToString());
			//					return;
			//				}
			//			}
			//
			//			MessageBox.Show("fehler");

//			// Get host name
//			string strHostName = Dns.GetHostName();
//
//			// Find host by name
//			IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
//
//		    string result = "blubb\n";
//
//			// Enumerate IP addresses
//			foreach (IPAddress ipaddress in iphostentry.AddressList)
//			{
//				result += ipaddress.ToString() + "\n";
//			}
//
//		    MessageBox.Show(result);
	    }

	    private void ShowAboutDialog()
        {
            viewModelCommunication.Send(new ShowDisabledOverlay());

            var dialogWindow = dialogBuilder.BuildWindow();
            dialogWindow.ShowDialog();

            viewModelCommunication.Send(new HideDisabledOverlay());
        }

        public ICommand ShowOverview { get; }
        public ICommand ShowSearch   { get; }
        public ICommand ShowOptions  { get; }
        public ICommand Logout       { get; }
        public ICommand ShowAbout    { get; }



        public IConnectionStatusViewModel ConnectionStatusViewModel { get; }

        protected override void CleanUp()
        {
        }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
