using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.XMLDataStores;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamPersistance
{
	// TODO: wenn monat mal geladen wurde, markieren und nicht wieder versuchen zu laden beim nächsten mal

	public class StreamPersistenceService : IStreamPersistenceService
    {
        private readonly string basePath;
		private readonly int maxCachedDays;	// TODO: implement caching
		private readonly IXmlEventStreamPersistanceService eventStreamPersistenceService;
		private readonly IConfigurationRepository config;


		private readonly IDictionary<AggregateIdentifier, EventStream<AggregateIdentifier>> cachedEventStreams; 		 

        public StreamPersistenceService(IXmlEventStreamPersistanceService eventStreamPersistenceService, 
									    IConfigurationRepository config, 
										string basePath,
										int maxCachedDays)
        {
	        this.eventStreamPersistenceService = eventStreamPersistenceService;
	        this.config = config;
            this.basePath = basePath;
	        this.maxCachedDays = maxCachedDays;

			cachedEventStreams = new Dictionary<AggregateIdentifier, EventStream<AggregateIdentifier>>();
        }


        public EventStream<AggregateIdentifier> GetEventStream(AggregateIdentifier identifier)
        {
	        if (cachedEventStreams.ContainsKey(identifier))
	        {
		        var eventStreamToDeliver = cachedEventStreams[identifier];

		        if (eventStreamToDeliver.EventCount == 0)		        
			        CreateNewEventStream(identifier);
		        
				return cachedEventStreams[identifier];
	        }

	        if (cachedEventStreams.Keys.Any(aggregateId => aggregateId.MedicalPracticeId == identifier.MedicalPracticeId &&
	                                                       aggregateId.Date.Month == identifier.Date.Month))
	        {
		        CreateNewEventStream(identifier);
				return cachedEventStreams[identifier];
			}

	        LoadMonth(identifier);

	        if (!cachedEventStreams.ContainsKey(identifier))	        
				CreateNewEventStream(identifier);	        

	        return cachedEventStreams[identifier];
        }

		private void LoadMonth(AggregateIdentifier identifier)
		{
			var newStreams = eventStreamPersistenceService.Load(GetFilePath(identifier));

			foreach (var eventStream in newStreams)
			{
				cachedEventStreams.Add(eventStream.Id, eventStream);
			}
		}

		public void FillCacheInitially()
		{
			foreach (var medicalPractice in config.GetAllMedicalPractices())
			{
				LoadMonth(new AggregateIdentifier(new Date(DateTime.Now.AddMonths(-1)), medicalPractice.Id));
				LoadMonth(new AggregateIdentifier(new Date(DateTime.Today),             medicalPractice.Id));				
				LoadMonth(new AggregateIdentifier(new Date(DateTime.Now.AddMonths(+1)), medicalPractice.Id));
				LoadMonth(new AggregateIdentifier(new Date(DateTime.Now.AddMonths(+2)), medicalPractice.Id));
			}
		}

		public void PersistStreams()
		{
			IDictionary<Guid, IList<EventStream<AggregateIdentifier>>> sortedByPratice = new Dictionary<Guid, IList<EventStream<AggregateIdentifier>>>();

			foreach (var eventStream in cachedEventStreams.Values)
			{
				var medPracticeId = eventStream.Id.MedicalPracticeId;

				if (!sortedByPratice.ContainsKey(medPracticeId))
					sortedByPratice.Add(medPracticeId, new List<EventStream<AggregateIdentifier>>());

				sortedByPratice[medPracticeId].Add(eventStream);
			}

			foreach (var practiceEventStreams in sortedByPratice)
			{
				IDictionary<Date, IList<EventStream<AggregateIdentifier>>> sortedByMonthAndYear 
					= new Dictionary<Date, IList<EventStream<AggregateIdentifier>>>();

				foreach (var eventStream in practiceEventStreams.Value)
				{
					var date = new Date(1, eventStream.Id.Date.Month, eventStream.Id.Date.Year);

					if (!sortedByMonthAndYear.ContainsKey(date))
						sortedByMonthAndYear.Add(date, new List<EventStream<AggregateIdentifier>>());

					sortedByMonthAndYear[date].Add(eventStream);
				}

				foreach (var eventStreamListForMonthAndYear in sortedByMonthAndYear.Values)
				{
					var filteredList = eventStreamListForMonthAndYear.Where(eventStream => eventStream.EventCount > 0).ToList();

					if (filteredList.Count > 0)
					{
						eventStreamPersistenceService.Persist(filteredList, GetFilePath(filteredList[0].Id));
					}					
				}
			}						
		}

		private void CreateNewEventStream(AggregateIdentifier identifier)
		{
			var newEventStream = new EventStream<AggregateIdentifier>(new AggregateIdentifier(identifier.Date,
																							  identifier.MedicalPracticeId,
																							  config.GetLatestVersionFor(identifier.MedicalPracticeId)));
			if (cachedEventStreams.ContainsKey(identifier))
				cachedEventStreams.Remove(identifier);

			cachedEventStreams.Add(identifier, newEventStream);
		} 

		private string GetFilePath(AggregateIdentifier identifier)
		{
			var folderPath = $@"{basePath}\{identifier.MedicalPracticeId}\{identifier.Date.Year}";

			if (!Directory.Exists(folderPath))			
				Directory.CreateDirectory(folderPath);
			
			return $@"{folderPath}\{identifier.Date.Month}.xml";
		}       
    }
}