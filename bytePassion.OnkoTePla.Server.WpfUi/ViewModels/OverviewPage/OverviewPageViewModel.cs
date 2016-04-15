using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
	internal class OverviewPageViewModel : ViewModel, IOverviewPageViewModel
    {
		private readonly IConnectionService connectionService;

		private string connectionStatusText;		
		private bool isConnectionActive;

		public OverviewPageViewModel(IConnectionService connectionService)
		{
			this.connectionService = connectionService;

			connectionService.ConnectionStatusChanged += OnConnectionStatusChanged;
			OnConnectionStatusChanged();
		}

		private void OnConnectionStatusChanged()
		{
			IsConnectionActive = connectionService.IsConnectionActive;

			ConnectionStatusText = connectionService.IsConnectionActive 
										? "active" 
										: "inactive";
		}

		public string ConnectionStatusText
		{
			get { return connectionStatusText; }
			private set { PropertyChanged.ChangeAndNotify(this, ref connectionStatusText, value); }
		}

		public bool IsConnectionActive
		{
			get { return isConnectionActive; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isConnectionActive, value); }
		}
		
		protected override void CleanUp()
		{
			connectionService.ConnectionStatusChanged -= OnConnectionStatusChanged;
		}
        public override event PropertyChangedEventHandler PropertyChanged;		
    }
}
