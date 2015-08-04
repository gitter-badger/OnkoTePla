using System;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;

using static bytePassion.OnkoTePla.Client.WPFVisualization.GlobalAccess.Global;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector
{
	public class DateSelectorViewModel : IDateSelectorViewModel
	{
		private readonly IConfigurationReadRepository configuration;

		private readonly GlobalState<Date> selectedDateState;
		private readonly GlobalState<Tuple<Guid, uint>> displayedPracticeState;

		private Date selectedDate;

		public DateSelectorViewModel(IConfigurationReadRepository configuration)
		{
			selectedDateState      = ViewModelCommunication.GetGlobalViewModelVariable<Date>             (AppointmentGridSelectedDateVariable);
			displayedPracticeState = ViewModelCommunication.GetGlobalViewModelVariable<Tuple<Guid, uint>>(AppointmentGridDisplayedPracticeVariable);

			this.configuration = configuration;

			this.selectedDateState.StateChanged += OnSelectedDateChanged;

			SelectedDate = selectedDateState.Value;

			SelectToday = new Command(() => SelectedDate = TimeTools.Today());
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

		public ICommand SelectToday { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
