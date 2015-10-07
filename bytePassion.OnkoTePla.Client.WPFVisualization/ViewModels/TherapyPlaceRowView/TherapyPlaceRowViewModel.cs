using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public class TherapyPlaceRowViewModel : DisposingObject,
											ITherapyPlaceRowViewModel											
	{
		private double gridWidth;

		public TherapyPlaceRowViewModel(IViewModelCommunication viewModelCommunication, 
									    IDataCenter dataCenter,
										TherapyPlace therapyPlace, 
										Color roomDisplayColor,										
										TherapyPlaceRowIdentifier identifier)
		{
			ViewModelCommunication = viewModelCommunication;
			DataCenter = dataCenter;

			RoomColor        = roomDisplayColor;		
			Identifier       = identifier;			
			TherapyPlaceName = therapyPlace.Name;

			AppointmentViewModels = new ObservableCollection<IAppointmentViewModel>();	
			
			viewModelCommunication.RegisterViewModelAtCollection<ITherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				Constants.TherapyPlaceRowViewModelCollection,
				this	
			);

			var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(identifier.PlaceAndDate.Date,
																		   identifier.PlaceAndDate.MedicalPracticeId);

			TimeSlotBegin = medicalPractice.HoursOfOpening.GetOpeningTime(identifier.PlaceAndDate.Date);
			TimeSlotEnd   = medicalPractice.HoursOfOpening.GetClosingTime(identifier.PlaceAndDate.Date);
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

		public override void CleanUp()
		{
			ViewModelCommunication.DeregisterViewModelAtCollection<TherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				Constants.TherapyPlaceRowViewModelCollection,
				this
			);
		}

		public IViewModelCommunication ViewModelCommunication { get; }
		public IDataCenter DataCenter { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}