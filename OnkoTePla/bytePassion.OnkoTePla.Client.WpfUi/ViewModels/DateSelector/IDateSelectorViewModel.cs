using bytePassion.Lib.TimeLib;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateSelector
{
    public interface IDateSelectorViewModel : IViewModel
	{
		Date SelectedDate { get; set; }

		ICommand SelectToday { get; }
	}
}
