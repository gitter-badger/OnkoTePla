using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector
{
	public class DateSelectorViewModel : IDateSelectorViewModel
	{		
		private readonly IGlobalState<Date> selectedDateState;

		private Date selectedDate;

		public DateSelectorViewModel(ViewModelCommunication<ViewModelMessage> viewModelCommunication)
		{
			selectedDateState = viewModelCommunication.GetGlobalViewModelVariable<Date>(
				AppointmentGridSelectedDateVariable
			);
						
			selectedDateState.StateChanged += OnSelectedDateChanged;

			SelectedDate = selectedDateState.Value;

			SelectToday = new Command(() => SelectedDate = TimeTools.Today());
		}

		private void OnSelectedDateChanged(Date date)
		{
			selectedDate = date;
			PropertyChanged.Notify(this, nameof(SelectedDate));
		}

		public Date SelectedDate
		{
			get { return selectedDate; }
			set
			{
				if (value != selectedDate)
				{					
					selectedDateState.Value = value;
					PropertyChanged.ChangeAndNotify(this, ref selectedDate, value);					
				}
			}
		}

		public ICommand SelectToday { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
