using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector
{
	public interface IDateSelectorViewModel : IViewModel
	{
		Date SelectedDate { get; set; }

		ICommand SelectToday { get; }

		// bool IsMinimized { get; }
	}
}
