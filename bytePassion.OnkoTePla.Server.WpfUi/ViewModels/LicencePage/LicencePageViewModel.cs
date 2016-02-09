using System.ComponentModel;
using bytePassion.Lib.WpfLib.ViewModelBase;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage
{
	internal class LicencePageViewModel : ViewModel, 
                                          ILicencePageViewModel
    {
        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
