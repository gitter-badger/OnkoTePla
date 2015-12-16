using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid.Helper;
using bytePassion.OnkoTePla.Core.Domain;
using System;
using System.Collections.ObjectModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid
{

    public interface ITimeGridViewModel : IViewModelCollectionItem<AggregateIdentifier>,
										  IDisposable,
										  IViewModelMessageHandler<NewSizeAvailable>,
										  IViewModelMessageHandler<Dispose>
	{
		ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; }
		ObservableCollection<TimeSlotLine>  TimeSlotLines  { get; }
	}
}