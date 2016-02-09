using System.ComponentModel;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientsPage
{
	internal class PatientsPageViewModel : ViewModel, IPatientsPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
