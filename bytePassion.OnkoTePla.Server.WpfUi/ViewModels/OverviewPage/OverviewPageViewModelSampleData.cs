using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
	internal class OverviewPageViewModelSampleData : IOverviewPageViewModel
    {
		public OverviewPageViewModelSampleData()
		{
			ConnectionStatusText  = "inactive";			
			IsConnectionActive = false;
		}

		public string ConnectionStatusText { get; }
		public bool   IsConnectionActive   { get; }		

		public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;		
    }
}