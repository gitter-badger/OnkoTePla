using System.ComponentModel;
using bytePassion.Lib.WpfLib.ViewModelBase;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage
{
	internal class AboutPageViewModel : ViewModel, IAboutPageViewModel
    {
		public AboutPageViewModel(string versionNumber)
		{
			VersionNumber = versionNumber;
		}

		public string VersionNumber { get; }

		protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;		
    }
}
