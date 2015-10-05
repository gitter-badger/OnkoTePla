using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewDragAdorner
{
	public class AppointmentViewDragAdornerViewModel : IAppointmentViewDragAdornerViewModel
	{
		private bool dropPossible;

		public AppointmentViewDragAdornerViewModel(string content)
		{
			Content = content;
			DropPossible = false;
		}

		public bool DropPossible
		{
			get { return dropPossible; }
			set { PropertyChanged.ChangeAndNotify(this, ref dropPossible, value); }
		}

		public string Content { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}

}
