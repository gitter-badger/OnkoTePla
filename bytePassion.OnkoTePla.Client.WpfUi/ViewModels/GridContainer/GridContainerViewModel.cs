using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.AppointmentGridViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
{
    public class GridContainerViewModel : ViewModel, 
                                          IGridContainerViewModel
	{
		private readonly IAppointmentGridViewModelBuilder appointmentGridViewModelBuilder;		
         
		private readonly IGlobalStateReadOnly<Date> selectedDateVariable;
		private readonly IGlobalStateReadOnly<Guid> selectedMedicalPracticeIdVariable;
	    private readonly IGlobalState<Size> appointmentGridSizeVariable;
	   		

		private readonly IDictionary<AggregateIdentifier, IAppointmentGridViewModel> cachedAppointmentGridViewModels; 

		private int                  currentDisplayedAppointmentGridIndex;
		private AggregateIdentifier? currentDisplayedAppointmentGridIdentifier;
		
		public GridContainerViewModel(IViewModelCommunication viewModelCommunication,
                                      IGlobalStateReadOnly<Date> selectedDateVariable, 
                                      IGlobalStateReadOnly<Guid> selectedMedicalPracticeIdVariable,
									  IGlobalState<Size> appointmentGridSizeVariable,									  
                                      IEnumerable<AggregateIdentifier> initialGridViewModelsToCache,									  
									  int maximumCashedGrids, 
									  IAppointmentGridViewModelBuilder appointmentGridViewModelBuilder /* TODO */)
		{
			// TODO caching implementieren

			
			ViewModelCommunication = viewModelCommunication;
		    this.selectedDateVariable = selectedDateVariable;
		    this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;
		    this.appointmentGridSizeVariable = appointmentGridSizeVariable;		    
			this.appointmentGridViewModelBuilder = appointmentGridViewModelBuilder;

			LoadedAppointmentGrids          = new ObservableCollection<IAppointmentGridViewModel>();
			cachedAppointmentGridViewModels = new Dictionary<AggregateIdentifier, IAppointmentGridViewModel>();

			currentDisplayedAppointmentGridIdentifier = null;			
			
			foreach (var identifier in initialGridViewModelsToCache)
			{
				AddGridViewModel(identifier);
			}

			selectedDateVariable.StateChanged              += OnSelectedDateStateChanged;
			selectedMedicalPracticeIdVariable.StateChanged += OnDisplayedPracticeStateChanged;

			ShowGridViewModel(new AggregateIdentifier(selectedDateVariable.Value, selectedMedicalPracticeIdVariable.Value));			
		}		

		public ObservableCollection<IAppointmentGridViewModel> LoadedAppointmentGrids { get; }
		public IViewModelCommunication ViewModelCommunication { get; }

		public int CurrentDisplayedAppointmentGridIndex
		{
			get { return currentDisplayedAppointmentGridIndex; }
			private set { PropertyChanged.ChangeAndNotify(this, ref currentDisplayedAppointmentGridIndex, value); }
		}

		public Lib.Types.SemanticTypes.Size ReportedGridSize
		{
			set { appointmentGridSizeVariable.Value = value; }
		}

		private void AddGridViewModel(AggregateIdentifier identifier)
		{
			if (!cachedAppointmentGridViewModels.ContainsKey(identifier))
			{
				var gridViewModel = appointmentGridViewModelBuilder.Build(identifier);

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
			var newIdentifier = new AggregateIdentifier(selectedDateVariable.Value, medicalPracticeId);
			ShowGridViewModel(newIdentifier);
		}

		private void OnSelectedDateStateChanged (Date date)
		{			
			var newIdentifier = new AggregateIdentifier(date, selectedMedicalPracticeIdVariable.Value);
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

	    protected override void CleanUp()
	    {
	        selectedDateVariable.StateChanged              -= OnSelectedDateStateChanged;
			selectedMedicalPracticeIdVariable.StateChanged -= OnDisplayedPracticeStateChanged;
	    }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
