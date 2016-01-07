using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Readmodels;

namespace bytePassion.OnkoTePla.Client.DataAndService.Data
{

	public interface IDataCenter
    {       
        AppointmentsOfADayReadModel     GetAppointmentsOfADayReadModel    (AggregateIdentifier identifier);
        AppointmentsOfAPatientReadModel GetAppointmentsOfAPatientReadModel(Guid                patientId);

		MedicalPractice GetMedicalPracticeById          (Guid medicalPracticeId);
        MedicalPractice GetMedicalPracticeByIdAndDate   (Guid medicalPracticeId, Date date);
        MedicalPractice GetMedicalPracticeByIdAndVersion(Guid medicalPracticeId, uint version);        

        IEnumerable<MedicalPractice> GetAllMedicalPractices();
        IEnumerable<Patient>         GetAllPatients();

		void SendCommand<TDomainCommand>(TDomainCommand command) where TDomainCommand : DomainCommand;

		void PersistEventstore(); // TODO: just for testing
    }
}