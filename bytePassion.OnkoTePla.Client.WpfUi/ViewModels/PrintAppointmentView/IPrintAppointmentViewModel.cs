using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PrintAppointmentView
{
	internal interface IPrintAppointmentViewModel : IViewModel
	{
		Time BeginTime { get; }
		Time EndTime   { get; }

		string PatientDisplayName { get; }
	}
}