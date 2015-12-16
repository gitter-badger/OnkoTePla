using System.ComponentModel;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage
{
    internal class LicencePageViewModel : ViewModel, 
                                          ILicencePageViewModel
    {
        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
