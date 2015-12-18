using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.Model;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Core.Domain;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid
{
    internal class ClosedDayGridViewModel : ViewModel, 
										    IAppointmentGridViewModel
	{
		private bool isActive;
		
		private readonly IViewModelCommunication viewModelCommunication;		         
		private readonly IGlobalStateReadOnly<Size> appointmentGridSizeVariable;
		
         
		public ClosedDayGridViewModel (AggregateIdentifier identifier,
									   IDataCenter dataCenter,										
									   IViewModelCommunication viewModelCommunication,
									   IGlobalStateReadOnly<Size> appointmentGridSizeVariable)
		{			
			this.viewModelCommunication = viewModelCommunication;
		    this.appointmentGridSizeVariable = appointmentGridSizeVariable;

		    IsActive = false;

			viewModelCommunication.RegisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				Constants.AppointmentGridViewModelCollection,
				this
			);
			
			appointmentGridSizeVariable.StateChanged += OnGridSizeChanged;

			var readModel = dataCenter.GetAppointmentsOfADayReadModel(identifier);

			Identifier = readModel.Identifier; // because now the identifier contains the correct Version
			
			readModel.Dispose();

			TimeGridViewModel = new TimeGridViewModel(Identifier, viewModelCommunication,
													  dataCenter, appointmentGridSizeVariable.Value);

			TherapyPlaceRowViewModels = new ObservableCollection<ITherapyPlaceRowViewModel>();

			PracticeIsClosedAtThisDay = true;
		}				

		
		private void OnGridSizeChanged (Size newGridSize)
		{
			if (IsActive)
			{
				viewModelCommunication.SendTo(
					Constants.TimeGridViewModelCollection,
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
			OnGridSizeChanged(appointmentGridSizeVariable.Value);
		}

		public void Process (Deactivate message)
		{
			IsActive = false;
		}
        
		public void Process(DeleteAppointment message)
		{
			throw new Exception("internal error");
		}
		
		public void Process(SendCurrentChangesToCommandBus message)
		{
			throw new Exception("internal error");
		}		

		public void Process(CreateNewAppointmentFromModificationsAndSendToCommandBus message)
		{
			throw new Exception("internal error");
		}

        protected override void CleanUp()
        {
            appointmentGridSizeVariable.StateChanged -= OnGridSizeChanged;

            viewModelCommunication.DeregisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
                Constants.AppointmentGridViewModelCollection,
                this
            );

            viewModelCommunication.SendTo(
                Constants.TimeGridViewModelCollection,
                Identifier,
                new Dispose()
            );
        }

        public override event PropertyChangedEventHandler PropertyChanged;
	}
}