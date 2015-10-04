using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid
{
	public class ClosedDayGridViewModel : DisposingObject, 
										  IAppointmentGridViewModel
	{
		private bool isActive;
		
		private readonly IViewModelCommunication viewModelCommunication;
		

		private readonly IGlobalState<Size> globalGridSizeVariable;
		

		public ClosedDayGridViewModel (AggregateIdentifier identifier,
									   IDataCenter dataCenter,										
									   IViewModelCommunication viewModelCommunication)
		{			
			this.viewModelCommunication = viewModelCommunication;
			
			IsActive = false;

			viewModelCommunication.RegisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection,
				this
			);

			globalGridSizeVariable = viewModelCommunication.GetGlobalViewModelVariable<Size>(
				AppointmentGridSizeVariable
				);
			globalGridSizeVariable.StateChanged += OnGridSizeChanged;

			var readModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(identifier);

			Identifier = readModel.Identifier; // because now the identifier contains the correct Version
			
			readModel.Dispose();

			TimeGridViewModel = new TimeGridViewModel(Identifier, viewModelCommunication,
													  dataCenter, globalGridSizeVariable.Value);

			TherapyPlaceRowViewModels = new ObservableCollection<ITherapyPlaceRowViewModel>();

			PracticeIsClosedAtThisDay = true;
		}				

		
		private void OnGridSizeChanged (Size newGridSize)
		{
			if (IsActive)
			{
				viewModelCommunication.SendTo(
					TimeGridViewModelCollection,
					Identifier,
					new NewSizeAvailable(newGridSize)
				);			
			}
		}

		public AggregateIdentifier Identifier { get; }

		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; }

		public ITimeGridViewModel TimeGridViewModel { get; }

		public bool PracticeIsClosedAtThisDay { get; }

		public bool IsActive
		{
			get { return isActive; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isActive, value); }
		}


		public void Process (Activate message)
		{
			IsActive = true;
			OnGridSizeChanged(globalGridSizeVariable.Value);
		}

		public void Process (Deactivate message)
		{
			IsActive = false;
		}

		public override void CleanUp ()
		{
			globalGridSizeVariable.StateChanged -= OnGridSizeChanged;
			
			viewModelCommunication.DeregisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection,
				this
			);

			viewModelCommunication.SendTo(
				TimeGridViewModelCollection,
				Identifier,
				new Dispose()
			);						
		}

		public void Process(DeleteAppointment message)
		{
			throw new Exception("internal error");
		}
		
		public void Process(SendCurrentChangesToCommandBus message)
		{
			throw new Exception("internal error");
		}		

		public void Process(CreateNewAppointmentAndSendToCommandBus message)
		{
			throw new Exception("internal error");
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}