using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using static bytePassion.OnkoTePla.Client.WPFVisualization.GlobalAccess.Global;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
{
	public class GridContainerViewModel : IGridContainerViewModel
	{
		private GlobalState<Date> selectedDateState;
		private GlobalState<Tuple<Guid, uint>> displayedPracticeState;

		public GridContainerViewModel()
		{
			selectedDateState = ViewModelCommunication.GetGlobalViewModelVariable<Date>
				(AppointmentGridSelectedDateVariable
			);

			selectedDateState.StateChanged += SelectedDateStateOnStateChanged;

			displayedPracticeState = ViewModelCommunication.GetGlobalViewModelVariable<Tuple<Guid, uint>>(
				AppointmentGridDisplayedPracticeVariable
			);
		}

		private void SelectedDateStateOnStateChanged(Date date)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<AppointmentGridViewModel> LoadedAppointmentGrids { get; }
		public AppointmentGridViewModel CurrentDisplayedAppointmentGridViewModel { get; }
	}
}
