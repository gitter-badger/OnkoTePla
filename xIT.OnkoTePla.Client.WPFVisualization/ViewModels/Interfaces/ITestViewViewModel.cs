using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.Appointments;
using xIT.OnkoTePla.Contracts.Infrastructure;
using xIT.OnkoTePla.Contracts.Patients;


namespace xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	internal interface ITestViewViewModel
	{
		IReadOnlyList<TherapyPlace> TherapyPlaces { get; }
		IReadOnlyList<Patient>      Patients      { get; }
		IReadOnlyList<Appointment>  Appointments  { get; } 
	}
}
