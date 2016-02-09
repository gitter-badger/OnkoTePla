using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Readmodels;
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
		 
		public ClientReadModelRepository (IClientEventBus eventBus,										    
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
								

		public void RequestAppointmentsOfADayReadModel(Action<AppointmentsOfADayReadModel> readModelAvailable, 
													   AggregateIdentifier id, Action<string> errorCallback)
		{
			if (cachedReadmodels.ContainsKey(id))
			{
				readModelAvailable(cachedReadmodels[id]);
				return;
			}

			connectionService.RequestAppointmentsOfADay(
				(appointments, aggregateId, aggregateVersion) =>
				{															
					medicalPracticeRepository.RequestMedicalPractice(
						practice =>
						{
							var newReadModel = new AppointmentsOfADayReadModel(eventBus,
																			   patientsRepository,
																			   practice,
																			   appointments,
																			   aggregateId,
																			   aggregateVersion,
																			   errorCallback);
							if (!cachedReadmodels.ContainsKey(aggregateId))							
								cachedReadmodels.Add(aggregateId, newReadModel);

							readModelAvailable(cachedReadmodels[id]);
						},
						aggregateId.MedicalPracticeId, 
						aggregateId.PracticeVersion, 
						errorCallback							
					);																				
				},
				id.Date,
				id.MedicalPracticeId,
				uint.MaxValue,
				errorCallback
			);
		}

		public void RequestAppointmentSetOfADay(Action<FixedAppointmentSet> appointmentSetAvailable, 
												AggregateIdentifier id, uint aggregateVersionLimit, Action<string> errorCallback)
		{
			if (aggregateVersionLimit == uint.MaxValue)
			{
				if (cachedReadmodels.ContainsKey(id))
				{
					var readModel = cachedReadmodels[id];
					appointmentSetAvailable(new FixedAppointmentSet(readModel.Identifier, 
																	readModel.AggregateVersion, 
																	readModel.Appointments));					
					return;
				}

				connectionService.RequestAppointmentsOfADay(
					(appointments, aggregateId, aggregateVersion) =>
					{
						medicalPracticeRepository.RequestMedicalPractice(
							practice =>
							{
								var newReadModel = new AppointmentsOfADayReadModel(eventBus,
																			   patientsRepository,
																			   practice,
																			   appointments,
																			   aggregateId,
																			   aggregateVersion,
																			   errorCallback);
								if (!cachedReadmodels.ContainsKey(aggregateId))
									cachedReadmodels.Add(aggregateId, newReadModel);

								appointmentSetAvailable(new FixedAppointmentSet(newReadModel.Identifier,
																	            newReadModel.AggregateVersion,
																	            newReadModel.Appointments));
							},
							aggregateId.MedicalPracticeId,
							aggregateId.PracticeVersion,
							errorCallback
						);
					},
					id.Date,
					id.MedicalPracticeId,
					uint.MaxValue,
					errorCallback
				);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}