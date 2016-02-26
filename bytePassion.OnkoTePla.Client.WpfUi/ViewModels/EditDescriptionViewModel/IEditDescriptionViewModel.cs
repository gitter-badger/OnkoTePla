using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel
{
	internal interface IEditDescriptionViewModel : IViewModel
	{
		ICommand Cancel { get; }
		ICommand Accept { get; }

		string Description { get; set; }
	}
}