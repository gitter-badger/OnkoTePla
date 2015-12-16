using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Model
{

	public interface IDataCenter {

		IConfigurationReadRepository Configuration       { get; }
		IPatientReadRepository       Patients            { get; }
		IReadModelRepository         ReadModelRepository { get; }
		SessionInformation           SessionInfo         { get; }

		MedicalPractice GetMedicalPracticeByDateAndId(Date date, Guid medicalPracticeId);
	}

}