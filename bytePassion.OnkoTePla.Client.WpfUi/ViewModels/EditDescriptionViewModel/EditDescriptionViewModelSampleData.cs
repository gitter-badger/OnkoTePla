using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel
{
	internal class EditDescriptionViewModelSampleData : IEditDescriptionViewModel
	{
		public EditDescriptionViewModelSampleData()
		{
			Description = "sampleText";
		}

		public ICommand Cancel => null;
		public ICommand Accept => null;

		public string Description { get; set; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;			
	}
}