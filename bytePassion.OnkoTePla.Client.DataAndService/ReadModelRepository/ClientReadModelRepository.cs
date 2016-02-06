using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Core.Domain;
using AppointmentsOfADayReadModel = bytePassion.OnkoTePla.Client.DataAndService.Readmodels.AppointmentsOfADayReadModel;

namespace bytePassion.OnkoTePla.Client.DataAndService.ReadModelRepository
{
	public class ClientReadModelRepository : IClientReadModelRepository
	{		
		private readonly IClientEventBus eventBus;		
		private readonly IClientPatientRepository patientsRepository;
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly IConnectionService connectionService;
		 
		private readonly IDictionary<AggregateIdentifier, AppointmentsOfADayReadModel> cachedReadmodels; 

		internal ClientReadModelRepository (IClientEventBus eventBus,										    
										    IClientPatientRepository patientsRepository,
											IClientMedicalPracticeRepository medicalPracticeRepository,
										    IConnectionService connectionService)
		{
			this.eventBus = eventBus;			
			this.patientsRepository = patientsRepository;
			this.medicalPracticeRepository = medicalPracticeRepository;
			this.connectionService = connectionService;

			cachedReadmodels = new ConcurrentDictionary<AggregateIdentifier, AppointmentsOfADayReadModel>();
		}
				

		public AppointmentsOfADayReadModel GetAppointmentsOfADayReadModel(AggregateIdentifier id)
		{
			if (cachedReadmodels.ContainsKey(id))
				return cachedReadmodels[id];

			return null;
		}

		public bool IsAppointmentsOfADayReadModelAvailable(AggregateIdentifier id)
		{
			return cachedReadmodels.ContainsKey(id);
		}

		public void RequestAppointmentsOfADayReadModel(Action<AppointmentsOfADayReadModel> readModelAvailable, 
													   AggregateIdentifier id, Action<string> errorCallback)
		{
			connectionService.RequestAppointmentsOfADay(
				(appointments, aggregateId) =>
				{
					if (IsAppointmentsOfADayReadModelAvailable(aggregateId))
						cachedReadmodels.Remove(aggregateId);


					AppointmentsOfADayReadModel newReadModel = null;

					if (medicalPracticeRepository.IsMedicalPracticeAvailable(aggregateId.MedicalPracticeId, aggregateId.PracticeVersion))
					{
						newReadModel = new AppointmentsOfADayReadModel(eventBus,
																	   patientsRepository,
																	   medicalPracticeRepository.GetMedicalPractice(aggregateId.MedicalPracticeId, 
																													aggregateId.PracticeVersion),
																	   appointments,
																	   aggregateId,
																	   errorCallback);
					}
					else
					{
						medicalPracticeRepository.RequestMedicalPractice(
							practice =>
							{
								newReadModel = new AppointmentsOfADayReadModel(eventBus,
																			   patientsRepository,
																			   practice,
																			   appointments,
																			   aggregateId,
																			   errorCallback);
							},
							aggregateId.MedicalPracticeId, 
							aggregateId.PracticeVersion, 
							errorCallback							
						);
					}

					if (newReadModel == null)
					{
						errorCallback("loading readModel failed");
						return;
					}
					
					cachedReadmodels.Add(aggregateId, newReadModel);
					readModelAvailable(newReadModel);
				},
				id.Date,
				id.MedicalPracticeId,
				errorCallback
			);
		}		
	}
}