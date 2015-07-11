using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class DateSelectorViewModelSampleData : IDateSelectorViewModel
	{
		public DateSelectorViewModelSampleData()
		{
			SelectedDate = TimeTools.Today().DayBefore().DayBefore();
		}

		public Date     SelectedDate { get; set; }
		public ICommand SelectToday  { get { return null; }}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
