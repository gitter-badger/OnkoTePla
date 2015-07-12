using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IDateSelectorViewModel : IViewModelBase
	{
		Date SelectedDate { get; set; }

		ICommand SelectToday { get; }

		// bool IsMinimized { get; }
	}
}
