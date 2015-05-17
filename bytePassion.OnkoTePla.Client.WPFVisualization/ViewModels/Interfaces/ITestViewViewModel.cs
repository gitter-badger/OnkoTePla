using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	internal interface ITestViewViewModel
	{
		IReadOnlyList<TherapyPlace> TherapyPlaces { get; }
		IReadOnlyList<Patient>      Patients      { get; }
		IReadOnlyList<Appointment>  Appointments  { get; } 
	}
}
