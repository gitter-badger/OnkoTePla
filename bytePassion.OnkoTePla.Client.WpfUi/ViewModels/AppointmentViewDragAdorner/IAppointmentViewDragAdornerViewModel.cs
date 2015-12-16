namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentViewDragAdorner
{
    public interface IAppointmentViewDragAdornerViewModel : IViewModel
	{
		bool DropPossible { get; set; }		
		string Content { get; }
	}
}