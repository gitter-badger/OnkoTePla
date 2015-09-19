using System.Windows.Input;
using bytePassion.Lib.WpfUtils.Commands;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public class AddAppointmentDialogViewModel : IAddAppointmentDialogViewModel
	{

		public AddAppointmentDialogViewModel()
		{
			CloseDialog = new Command(() =>
			{
				
			});
		}

		public ICommand CloseDialog { get; }
	}
}
