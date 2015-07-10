using System;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class AppointmentViewModelSampleData : IAppointmentViewModel
	{
		public AppointmentViewModelSampleData()
			: this(0, 200)
		{			
		}

		public AppointmentViewModelSampleData(double canvasPosition, double viewElementLength)
		{
			PatientDisplayName = "Jerry Black";			

			CanvasPosition = canvasPosition;
			ViewElementLength = viewElementLength;

			OperatingMode = OperatingMode.Edit;
		}

		public ICommand DeleteAppointment { get { return null; }}
		public ICommand SwitchToEditMode  { get { return null; } }

		public string   PatientDisplayName  { get; private set; }		
		public double   CanvasPosition      { get; set; }
		public double   ViewElementLength   { get; set; }

		public Guid AppointmentId  { get { return new Guid(); }}
		public Guid TherapyPlaceId { get { return new Guid(); }}
		
		public OperatingMode OperatingMode  { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public void Dispose() {}
	}
}
