namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewDragAdorner
{
	public interface IAppointmentViewDragAdornerViewModel : IViewModel
	{
		bool DropPossible { get; set; }		
		string Content { get; }
	}
}