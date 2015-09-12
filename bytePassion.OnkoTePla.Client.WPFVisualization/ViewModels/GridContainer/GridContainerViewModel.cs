using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
{
	public class GridContainerViewModel : IGridContainerViewModel										  
	{
		private readonly IDataCenter dataCenter;
		private readonly ICommandBus  commandBus;		

		private readonly IGlobalState<Date> selectedDateState;
		private readonly IGlobalState<Guid> displayedPracticeState;		

		private readonly IDictionary<AggregateIdentifier, IAppointmentGridViewModel> cachedAppointmentGridViewModels; 

		private int                 currentDisplayedAppointmentGridIndex;
		private AggregateIdentifier? currentDisplayedAppointmentGridIdentifier;
		
		public GridContainerViewModel(IDataCenter dataCenter,
									  ICommandBus commandBus,
									  IViewModelCommunication viewModelCommunication,								  
                                      IEnumerable<AggregateIdentifier> initialGridViewModelsToCache,
									  int maximumCashedGrids /* TODO */)
		{
			// TODO caching implementieren

			this.dataCenter = dataCenter;
			this.commandBus = commandBus;
			ViewModelCommunication = viewModelCommunication;

			LoadedAppointmentGrids          = new ObservableCollection<IAppointmentGridViewModel>();
			cachedAppointmentGridViewModels = new Dictionary<AggregateIdentifier, IAppointmentGridViewModel>();

			currentDisplayedAppointmentGridIdentifier = null;

			selectedDateState = viewModelCommunication.GetGlobalViewModelVariable<Date>(
				AppointmentGridSelectedDateVariable
			);

			displayedPracticeState = viewModelCommunication.GetGlobalViewModelVariable<Guid>(
				AppointmentGridDisplayedPracticeVariable
			);
			
			foreach (var identifier in initialGridViewModelsToCache)
			{
				AddGridViewModel(identifier);
			}

			selectedDateState.StateChanged      += OnSelectedDateStateChanged;
			displayedPracticeState.StateChanged += OnDisplayedPracticeStateChanged;

			ShowGridViewModel(new AggregateIdentifier(selectedDateState.Value, displayedPracticeState.Value));			
		}		

		public ObservableCollection<IAppointmentGridViewModel> LoadedAppointmentGrids { get; }

		public int CurrentDisplayedAppointmentGridIndex
		{
			get { return currentDisplayedAppointmentGridIndex; }
			private set { PropertyChanged.ChangeAndNotify(this, ref currentDisplayedAppointmentGridIndex, value); }
		}

		private void AddGridViewModel(AggregateIdentifier identifier)
		{
			if (!cachedAppointmentGridViewModels.ContainsKey(identifier))
			{
				var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(identifier.Date, identifier.MedicalPracticeId);

				IAppointmentGridViewModel gridViewModel;

				if (medicalPractice.HoursOfOpening.IsOpen(identifier.Date))
					gridViewModel = new AppointmentGridViewModel(identifier, dataCenter, commandBus,ViewModelCommunication);
				else
					gridViewModel = new ClosedDayGridViewModel(identifier, dataCenter, ViewModelCommunication);

				cachedAppointmentGridViewModels.Add(identifier, gridViewModel);
				LoadedAppointmentGrids.Add(gridViewModel);
			}
		}

		private void RemoveGridViewModel(AggregateIdentifier identifier)
		{
			if (GridViewModelIsCached(identifier))
			{
				var gridViewModel = cachedAppointmentGridViewModels[identifier];
				cachedAppointmentGridViewModels.Remove(identifier);
				LoadedAppointmentGrids.Remove(gridViewModel);

				gridViewModel.Dispose();
			}
		}

		private bool GridViewModelIsCached(AggregateIdentifier identifier)
		{
			return cachedAppointmentGridViewModels.ContainsKey(identifier);
		}

		private int GetGridIndex(AggregateIdentifier identifier)
		{
			if (GridViewModelIsCached(identifier))
			{
				var gridViewModel = cachedAppointmentGridViewModels[identifier];
				return LoadedAppointmentGrids.IndexOf(gridViewModel);
			}

			throw new ArgumentException("viewModel is not cached");
		}				

		private void OnDisplayedPracticeStateChanged(Guid medicalPracticeId)
		{
			var newIdentifier = new AggregateIdentifier(selectedDateState.Value, medicalPracticeId);
			ShowGridViewModel(newIdentifier);
		}

		private void OnSelectedDateStateChanged (Date date)
		{			
			var newIdentifier = new AggregateIdentifier(date, displayedPracticeState.Value);
			ShowGridViewModel(newIdentifier);			
		}

		private void ActivateGridViewModel(AggregateIdentifier identifier)
		{
			ViewModelCommunication.SendTo(
				AppointmentGridViewModelCollection,
				identifier,
				new Activate()
			);
		}

		private void DeactivateGridViewModel (AggregateIdentifier identifier)
		{
			ViewModelCommunication.SendTo(
				AppointmentGridViewModelCollection,
				identifier,
				new Deactivate()
			);
		}

		private void ShowGridViewModel(AggregateIdentifier identifier)
		{			
			if (currentDisplayedAppointmentGridIdentifier.HasValue)
				DeactivateGridViewModel(currentDisplayedAppointmentGridIdentifier.Value);

			if (!GridViewModelIsCached(identifier))
				AddGridViewModel(identifier);

			CurrentDisplayedAppointmentGridIndex      = GetGridIndex(identifier);
			currentDisplayedAppointmentGridIdentifier = identifier;

			ActivateGridViewModel(identifier);
		}
		
		public event PropertyChangedEventHandler PropertyChanged;

		public IViewModelCommunication ViewModelCommunication { get; }
	}
}
