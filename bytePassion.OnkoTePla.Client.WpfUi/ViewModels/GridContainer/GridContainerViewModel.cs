using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentGridViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer
{
	internal class GridContainerViewModel : ViewModel, 
                                            IGridContainerViewModel
	{
		private readonly IAppointmentGridViewModelBuilder appointmentGridViewModelBuilder;
		private readonly Action<string> errorCallback;

		private readonly ISharedStateReadOnly<Date> selectedDateVariable;
		private readonly ISharedStateReadOnly<Guid> selectedMedicalPracticeIdVariable;
	    private readonly ISharedState<Size> appointmentGridSizeVariable;
	   		

		private readonly IDictionary<AggregateIdentifier, IAppointmentGridViewModel> cachedAppointmentGridViewModels; 

		private int                  currentDisplayedAppointmentGridIndex;
		private AggregateIdentifier? currentDisplayedAppointmentGridIdentifier;
		
		public GridContainerViewModel(IViewModelCommunication viewModelCommunication,
                                      ISharedStateReadOnly<Date> selectedDateVariable, 
                                      ISharedStateReadOnly<Guid> selectedMedicalPracticeIdVariable,
									  ISharedState<Size> appointmentGridSizeVariable,									  
                                      IEnumerable<AggregateIdentifier> initialGridViewModelsToCache,									  
									  int maximumCashedGrids, 
									  IAppointmentGridViewModelBuilder appointmentGridViewModelBuilder /* TODO */,
									  Action<string> errorCallback)
		{
			// TODO caching implementieren

			
			ViewModelCommunication = viewModelCommunication;
		    this.selectedDateVariable = selectedDateVariable;
		    this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;
		    this.appointmentGridSizeVariable = appointmentGridSizeVariable;		    
			this.appointmentGridViewModelBuilder = appointmentGridViewModelBuilder;
			this.errorCallback = errorCallback;

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
			set
			{
				if (value != null)
					appointmentGridSizeVariable.Value = value;
			}
		}

		private void AddGridViewModel(AggregateIdentifier identifier)
		{
			if (!cachedAppointmentGridViewModels.ContainsKey(identifier))
			{
				appointmentGridViewModelBuilder.RequestBuild(
					buildedViewModel =>
					{
						cachedAppointmentGridViewModels.Add(identifier, buildedViewModel);
						LoadedAppointmentGrids.Add(buildedViewModel);
					},
					identifier,
					errorCallback	
				);								
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
				Constants.AppointmentGridViewModelCollection,
				identifier,
				new Activate()
			);
		}

		private void DeactivateGridViewModel (AggregateIdentifier identifier)
		{
			ViewModelCommunication.SendTo(
				Constants.AppointmentGridViewModelCollection,
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
