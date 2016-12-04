using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PrintTherapyPlaceRow;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PrintAppointmentGrid
{
	internal interface IPrintAppointmentGridViewModel : IViewModel
	{
		ObservableCollection<IPrintTherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; }

		ITimeGridViewModel TimeGridViewModel { get; }
	}
}