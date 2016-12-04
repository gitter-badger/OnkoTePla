using bytePassion.Lib.WpfLib.ViewModelBase;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage
{
	internal interface IAboutPageViewModel : IViewModel
    {
       string VersionNumber { get; } 
    }
}