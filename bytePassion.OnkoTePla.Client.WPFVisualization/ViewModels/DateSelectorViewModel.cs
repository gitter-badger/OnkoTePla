using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.State;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class DateSelectorViewModel : IDateSelectorViewModel
	{
		private readonly GlobalState<Date> selectedDateState;

		private readonly ICommand selectTodayCommand;

		private Date selectedDate;

		public DateSelectorViewModel(GlobalState<Date> selectedDateState)
		{
			this.selectedDateState = selectedDateState;
			this.selectedDateState.StateChanged += OnSelectedDateChanged;

			SelectedDate = selectedDateState.Value;

			selectTodayCommand = new Command(() => SelectedDate = TimeTools.Today());
		}

		private void OnSelectedDateChanged(Date date)
		{
			SelectedDate = date;
		}


		public Date SelectedDate
		{
			get { return selectedDate; }
			set
			{
				if (value != selectedDate)
					selectedDateState.Value = value;

				PropertyChanged.ChangeAndNotify(this, ref selectedDate, value);
			}
		}

		public ICommand SelectToday
		{
			get { return selectTodayCommand; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
