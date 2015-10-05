using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewDragAdorner
{

	public class AppointmentViewDragAdornerViewModelSampleData : IAppointmentViewDragAdornerViewModel
	{
		public AppointmentViewDragAdornerViewModelSampleData()
		{
			DropPossible = true;
			Content = "John Doh";
		}

		public bool DropPossible { get; set; }
		public string Content { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}