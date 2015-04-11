using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.Appointments;
using xIT.OnkoTePla.Contracts.DataObjects;


namespace xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	internal interface ITestViewViewModel
	{
		IReadOnlyList<TherapyPlace> TherapyPlaces { get; }
		IReadOnlyList<Patient>      Patients      { get; }
		IReadOnlyList<Appointment>  Appointments  { get; } 
	}
}
