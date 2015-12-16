using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfLib.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector
{
    public class DateSelectorViewModel : ViewModel, 
                                         IDateSelectorViewModel
	{		
		private readonly IGlobalState<Date> selectedDateVariable;

		private Date selectedDate;

		public DateSelectorViewModel(IGlobalState<Date> selectedDateVariable)
		{
		    this.selectedDateVariable = selectedDateVariable;

            SelectToday = new Command(() => SelectedDate = TimeTools.Today());

            selectedDateVariable.StateChanged += OnSelectedDateChanged;

			SelectedDate = selectedDateVariable.Value;			
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
					selectedDateVariable.Value = value;
					PropertyChanged.ChangeAndNotify(this, ref selectedDate, value);					
				}
			}
		}

		public ICommand SelectToday { get; }

        protected override void CleanUp()
        {
            selectedDateVariable.StateChanged -= OnSelectedDateChanged;
        }        
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
