using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.Workflow;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using System;


namespace bytePassion.OnkoTePla.Client.WpfUi.Model
{

    internal interface IDataCenter {

		IConfigurationReadRepository Configuration       { get; }
		IPatientReadRepository       Patients            { get; }
		IReadModelRepository         ReadModelRepository { get; }
		SessionInformation           SessionInfo         { get; }
        IClientWorkflow               Workflow           { get; }

		MedicalPractice GetMedicalPracticeByDateAndId(Date date, Guid medicalPracticeId);
	}

}