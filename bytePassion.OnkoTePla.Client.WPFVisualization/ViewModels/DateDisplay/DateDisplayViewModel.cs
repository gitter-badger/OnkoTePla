using System;
using System.ComponentModel;
using System.Globalization;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay
{
	public class DateDisplayViewModel : IDateDisplayViewModel
	{
		private string date;

		public DateDisplayViewModel(IViewModelCommunication viewModelCommunication)
		{
			var selectedDateVariable = viewModelCommunication.GetGlobalViewModelVariable<Date>(
				AppointmentGridSelectedDateVariable
			);

			selectedDateVariable.StateChanged += OnSelectedDateVariableChanged;

			OnSelectedDateVariableChanged(selectedDateVariable.Value);
		}

		private void OnSelectedDateVariableChanged(Date newDate)
		{
			Date = newDate.GetDisplayString(new CultureInfo("de-DE"));
		}

        public string Date
		{
			get { return date; }
			private set { PropertyChanged.ChangeAndNotify(this, ref date, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
