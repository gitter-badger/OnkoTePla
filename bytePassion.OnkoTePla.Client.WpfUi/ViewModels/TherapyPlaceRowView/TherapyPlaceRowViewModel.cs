using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView
{
	internal class TherapyPlaceRowViewModel : ViewModel,
											  ITherapyPlaceRowViewModel
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ISharedState<AppointmentModifications> appointmentModificationsVariable;
		

		private double gridWidth;
		private bool isVisible;
		private AppointmentModifications appointmentModifications;		
		
		public TherapyPlaceRowViewModel(IViewModelCommunication viewModelCommunication, 									    
										TherapyPlace therapyPlace, 
										Color roomDisplayColor,										
										TherapyPlaceRowIdentifier identifier,
										AdornerControl adornerControl,
										Time timeSlotBegin,
										Time timeSlotEnd,
										ISharedState<AppointmentModifications> appointmentModificationsVariable)
		{
			this.viewModelCommunication = viewModelCommunication;
			this.appointmentModificationsVariable = appointmentModificationsVariable;						
			
			IsVisible		      = true;
			RoomColor             = roomDisplayColor;		
			Identifier            = identifier;			
			TherapyPlaceName      = therapyPlace.Name;
			AppointmentViewModels = new ObservableCollection<IAppointmentViewModel>();	
			
			viewModelCommunication.RegisterViewModelAtCollection<ITherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				Constants.TherapyPlaceRowViewModelCollection,
				this	
			);

			appointmentModificationsVariable.StateChanged += OnAppointmentModificationsChanged;
			OnAppointmentModificationsChanged(appointmentModificationsVariable.Value);

			TimeSlotBegin = timeSlotBegin;
			TimeSlotEnd   = timeSlotEnd;

			AdornerControl = adornerControl;			
		}
		

		private void OnAppointmentModificationsChanged(AppointmentModifications newAppointmentModifications)
		{
			AppointmentModifications = newAppointmentModifications;
		}

		public TherapyPlaceRowIdentifier Identifier { get; }

		public ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }		
		
		public Color  RoomColor        { get; }		
		public string TherapyPlaceName { get; }

		public Time TimeSlotBegin { get; }
		public Time TimeSlotEnd   { get; }

		public double GridWidth
		{
			get { return gridWidth; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gridWidth, value); }
		}

		public AdornerControl AdornerControl { get; }

		public AppointmentModifications AppointmentModifications
		{
			get { return appointmentModifications; }
			private set { PropertyChanged.ChangeAndNotify(this, ref appointmentModifications, value); }
		}		

		public bool IsVisible
		{
			get { return isVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isVisible, value); }
		}

		public void Process (NewSizeAvailable message)
		{
			GridWidth = message.NewSize.Width;
		}

		public void Process(AddAppointmentToTherapyPlaceRow message)
		{
			AppointmentViewModels.Add(message.AppointmentViewModelToAdd);
		}

		public void Process(RemoveAppointmentFromTherapyPlaceRow message)
		{
			AppointmentViewModels.Remove(message.AppointmentViewModelToRemove);
		}

		public void Process (SetVisibility message)
		{
			IsVisible = message.Visible;
		}

        protected override void CleanUp()
		{
			viewModelCommunication.DeregisterViewModelAtCollection<TherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				Constants.TherapyPlaceRowViewModelCollection,
				this
			);

			appointmentModificationsVariable.StateChanged -= OnAppointmentModificationsChanged;			
		}		

		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}