using System.ComponentModel;
using bytePassion.Lib.WpfLib.ViewModelBase;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientsPage
{
	internal class PatientsPageViewModel : ViewModel, IPatientsPageViewModel
	{
		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
