using System.ComponentModel;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage
{
    internal class InfrastructurePageViewModel : ViewModel, 
                                                 IInfrastructurePageViewModel
    {
                
        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
