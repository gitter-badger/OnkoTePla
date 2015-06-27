using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IAppointmentViewModel
	{
		string PatientDisplayName { get; }
		double FrameworkElementWidth { get; }
	}
}
