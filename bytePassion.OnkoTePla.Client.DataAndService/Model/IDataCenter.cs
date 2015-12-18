using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Readmodels;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Client.DataAndService.Model
{

    public interface IDataCenter
    {
        User LoggedInUser { get; }

        AppointmentsOfADayReadModel     GetAppointmentsOfADayReadModel    (AggregateIdentifier identifier);
        AppointmentsOfAPatientReadModel GetAppointmentsOfAPatientReadModel(Guid                patientId);

        MedicalPractice GetMedicalPracticeByIdAndDate   (Guid medicalPracticeId, Date date);
        MedicalPractice GetMedicalPracticeByIdAndVersion(Guid medicalPracticeId, uint version);
        MedicalPractice GetMedicalPracticeById          (Guid medicalPracticeId);

        IEnumerable<MedicalPractice> GetAllMedicalPractices();
        IEnumerable<Patient>         GetAllPatients();
    }

}