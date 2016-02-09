using System.ComponentModel;
using bytePassion.Lib.WpfLib.ViewModelBase;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
	internal class OverviewPageViewModel : ViewModel, IOverviewPageViewModel
    {        
        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }
}
