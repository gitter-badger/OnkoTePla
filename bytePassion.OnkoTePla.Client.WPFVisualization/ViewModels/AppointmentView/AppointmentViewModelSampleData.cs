using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public class AppointmentViewModelSampleData : IAppointmentViewModel
	{
		public AppointmentViewModelSampleData()
			: this(0, 200)
		{			
		}

		public AppointmentViewModelSampleData(double canvasLeftPosition, double viewElementLength)
		{
			PatientDisplayName = "Jerry Black";
			TimeSpan = "";
			AppointmentDate = "";
			Description = "";
			Room = "";		

			CanvasLeftPosition = canvasLeftPosition;
			ViewElementLength  = viewElementLength;

			OperatingMode = OperatingMode.Edit;
		}

		public ICommand DeleteAppointment => null;
		public ICommand SwitchToEditMode  => null;

		public string PatientDisplayName { get; }
		public string TimeSpan           { get; }
		public string AppointmentDate    { get; }
		public string Description        { get; }
		public string Room               { get; }

		public double CanvasLeftPosition { get; set; }
		public double ViewElementLength  { get; set; }
		

		public OperatingMode OperatingMode  { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public void Dispose() {}
	}
}
