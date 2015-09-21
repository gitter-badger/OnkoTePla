using System.Linq;
using System.Windows;
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
				var windows = Application.Current.Windows
												 .OfType<Views.AddAppointmentDialog>()
												 .ToList();

				if (windows.Count() == 1)
					windows[0].Close();
					
			});
		}

		public ICommand CloseDialog { get; }
	}
}
