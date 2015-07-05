using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
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
		}

		public ICommand DeleteAppointment { get { return null; }}

		public string   PatientDisplayName  { get; private set; }		
		public double   CanvasPosition      { get; set; }
		public double   ViewElementLength   { get; set; }

		public Time StartTime { get; set; }
		public Time EndTime   { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
