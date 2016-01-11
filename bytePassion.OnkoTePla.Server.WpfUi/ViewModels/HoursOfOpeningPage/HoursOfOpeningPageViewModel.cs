using System.ComponentModel;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage
{
	internal class HoursOfOpeningPageViewModel : ViewModel, 
												 IHoursOfOpeningPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
