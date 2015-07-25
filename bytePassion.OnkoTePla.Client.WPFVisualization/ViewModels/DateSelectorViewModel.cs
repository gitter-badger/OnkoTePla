using System;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class DateSelectorViewModel : IDateSelectorViewModel
	{
		private readonly IConfigurationReadRepository configuration;

		private readonly GlobalState<Date> selectedDateState;
		private readonly GlobalState<Tuple<Guid, uint>> displayedPracticeState;

		private readonly ICommand selectTodayCommand;

		private Date selectedDate;

		public DateSelectorViewModel(GlobalState<Date> selectedDateState,
									 GlobalState<Tuple<Guid, uint>> displayedPracticeState,
									 IConfigurationReadRepository configuration)
		{
			this.selectedDateState = selectedDateState;
			this.displayedPracticeState = displayedPracticeState;
			this.configuration = configuration;

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
				{
					var medicalPractice = configuration.GetMedicalPracticeByIdAndVersion(displayedPracticeState.Value.Item1, displayedPracticeState.Value.Item2);

					if (medicalPractice.HoursOfOpening.IsOpen(value))
					{
						selectedDateState.Value = value;
						PropertyChanged.ChangeAndNotify(this, ref selectedDate, value);
					}
				}
			}
		}

		public ICommand SelectToday
		{
			get { return selectTodayCommand; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
