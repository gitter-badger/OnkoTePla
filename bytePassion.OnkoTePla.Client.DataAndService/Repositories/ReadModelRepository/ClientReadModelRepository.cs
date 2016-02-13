﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
using bytePassion.OnkoTePla.Contracts.Domain;
using AppointmentsOfADayReadModel = bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.AppointmentsOfADayReadModel;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository
{
	public class ClientReadModelRepository : IClientReadModelRepository
	{		
		private readonly IClientEventBus eventBus;		
		private readonly IClientPatientRepository patientsRepository;
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly IConnectionService connectionService;
		 
		private readonly IDictionary<AggregateIdentifier, AppointmentsOfADayReadModel>     cachedDayReadmodels;
		private readonly IDictionary<Guid,                AppointmentsOfAPatientReadModel> cachedPatientReadmodel; 
		 
		public ClientReadModelRepository (IClientEventBus eventBus,										    
										    IClientPatientRepository patientsRepository,
											IClientMedicalPracticeRepository medicalPracticeRepository,
										    IConnectionService connectionService)
		{
			this.eventBus = eventBus;			
			this.patientsRepository = patientsRepository;
			this.medicalPracticeRepository = medicalPracticeRepository;
			this.connectionService = connectionService;

			cachedDayReadmodels    = new ConcurrentDictionary<AggregateIdentifier, AppointmentsOfADayReadModel>();
			cachedPatientReadmodel = new ConcurrentDictionary<Guid,                AppointmentsOfAPatientReadModel>();
		}
								

		public void RequestAppointmentsOfADayReadModel(Action<AppointmentsOfADayReadModel> readModelAvailable, 
													   AggregateIdentifier id, Action<string> errorCallback)
		{
			if (cachedDayReadmodels.ContainsKey(id))
			{
				readModelAvailable(cachedDayReadmodels[id]);
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
							if (!cachedDayReadmodels.ContainsKey(aggregateId))							
								cachedDayReadmodels.Add(aggregateId, newReadModel);

							readModelAvailable(cachedDayReadmodels[id]);
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
				if (cachedDayReadmodels.ContainsKey(id))
				{
					var readModel = cachedDayReadmodels[id];
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
								if (!cachedDayReadmodels.ContainsKey(aggregateId))
									cachedDayReadmodels.Add(aggregateId, newReadModel);

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

		public void RequestLastestReadModelVersion(Action<uint> readModelVersionAvailable, AggregateIdentifier id, Action<string> errorCallback)
		{
			if (cachedDayReadmodels.ContainsKey(id))
			{
				readModelVersionAvailable(cachedDayReadmodels[id].AggregateVersion);
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
							if (!cachedDayReadmodels.ContainsKey(aggregateId))
								cachedDayReadmodels.Add(aggregateId, newReadModel);

							readModelVersionAvailable(cachedDayReadmodels[id].AggregateVersion);
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

		public void RequestAppointmentsOfAPatientReadModel(Action<AppointmentsOfAPatientReadModel> readModelAvailable, Guid patientId, Action<string> errorCallback)
		{
			if (cachedPatientReadmodel.ContainsKey(patientId))
			{
				var readModel = cachedPatientReadmodel[patientId];
				readModelAvailable(readModel);
				return;
			}

			connectionService.RequestAppointmentsOfAPatient(
				apointments =>
				{
					var newReadModel = new AppointmentsOfAPatientReadModel(patientId, 
																		  eventBus, 
																		  apointments,
																		  patientsRepository);

					if (!cachedPatientReadmodel.ContainsKey(patientId))
						cachedPatientReadmodel.Add(patientId, newReadModel);
					
					readModelAvailable(cachedPatientReadmodel[patientId]);
				},
				patientId,
				errorCallback	
			);
		}
	}
}