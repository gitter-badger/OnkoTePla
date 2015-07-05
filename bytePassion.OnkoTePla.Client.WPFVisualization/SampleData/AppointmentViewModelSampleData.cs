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
			: this(default(Time), default(Time), 0, 200)
		{			
		}

		public AppointmentViewModelSampleData(Time startTime, Time endTime, 
											  double canvasPosition, double viewElementLength)
		{
			PatientDisplayName = "Jerry Black";
			Duration = new Duration(3600);

			StartTime = startTime == default(Time) ? new Time(7, 30) : startTime;
			EndTime   = endTime   == default(Time) ? new Time(7, 30) : endTime;

			CanvasPosition = canvasPosition;
			ViewElementLength = viewElementLength;
		}

		public ICommand DeleteAppointment { get { return null; }}

		public string   PatientDisplayName  { get; private set; }
		public Duration Duration            { get; private set; }
		public double   CanvasPosition      { get; set; }
		public double   ViewElementLength   { get; set; }

		public Time StartTime { get; set; }
		public Time EndTime   { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
