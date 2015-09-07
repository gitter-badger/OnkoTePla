using System;
using System.Collections.ObjectModel;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid
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