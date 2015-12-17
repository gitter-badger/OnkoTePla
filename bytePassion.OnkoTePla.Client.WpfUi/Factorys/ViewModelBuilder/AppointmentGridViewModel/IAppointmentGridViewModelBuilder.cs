using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentGridViewModel
{
    internal interface IAppointmentGridViewModelBuilder
	{
		IAppointmentGridViewModel Build(AggregateIdentifier identifier);
	}
}