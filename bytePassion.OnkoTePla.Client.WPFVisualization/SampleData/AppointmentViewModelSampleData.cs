using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class AppointmentViewModelSampleData : IAppointmentViewModel
	{
		public AppointmentViewModelSampleData()
		{
			PatientDisplayName = "Jerry Black";
			FrameworkElementWidth = 300;
		}

		public string PatientDisplayName    { get; private set; }
		public double FrameworkElementWidth { get; private set; }
	}
}
