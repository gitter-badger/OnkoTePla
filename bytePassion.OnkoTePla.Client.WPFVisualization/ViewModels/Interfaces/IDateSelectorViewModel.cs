using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IDateSelectorViewModel : INotifyPropertyChanged
	{
		Date SelectedDate { get; set; }

		ICommand SelectToday { get; }

		// bool IsMinimized { get; }
	}
}
