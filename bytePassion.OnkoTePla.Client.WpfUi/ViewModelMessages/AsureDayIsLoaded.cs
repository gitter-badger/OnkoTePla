using System;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Contracts.Domain;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
	public class AsureDayIsLoaded : ViewModelMessage
	{
		public AsureDayIsLoaded(AggregateIdentifier aggregateIdentifier, Action dayIsLoadedCallback)
		{
			AggregateIdentifier = aggregateIdentifier;
			DayIsLoadedCallback = dayIsLoadedCallback;
		}
		
		public AggregateIdentifier AggregateIdentifier { get; }
		public Action              DayIsLoadedCallback { get; } 
	}
}