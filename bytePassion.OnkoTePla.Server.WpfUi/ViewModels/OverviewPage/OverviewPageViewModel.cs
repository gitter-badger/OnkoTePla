using System.ComponentModel;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
	internal class OverviewPageViewModel : ViewModel, IOverviewPageViewModel
    {        
        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }
}
