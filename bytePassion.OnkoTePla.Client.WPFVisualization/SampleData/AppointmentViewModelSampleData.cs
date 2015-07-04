using System.ComponentModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class AppointmentViewModelSampleData : IAppointmentViewModel
	{
		public AppointmentViewModelSampleData()
			: this(default(Time), default(Time))
		{			
		}

		public AppointmentViewModelSampleData(Time startTime, Time endTime)
		{
			PatientDisplayName = "Jerry Black";
			Duration = new Duration(3600);

			StartTime = startTime == default(Time) ? new Time(7, 30) : startTime;
			EndTime   = endTime   == default(Time) ? new Time(7, 30) : endTime;
		}

		public string PatientDisplayName    { get; private set; }
		public Duration Duration { get; private set; }

		public Time StartTime { get; set; }
		public Time EndTime   { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
