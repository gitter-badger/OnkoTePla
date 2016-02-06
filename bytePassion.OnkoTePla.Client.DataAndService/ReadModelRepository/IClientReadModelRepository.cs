using System;
using bytePassion.OnkoTePla.Core.Domain;
using AppointmentsOfADayReadModel = bytePassion.OnkoTePla.Client.DataAndService.Readmodels.AppointmentsOfADayReadModel;

namespace bytePassion.OnkoTePla.Client.DataAndService.ReadModelRepository
{
	public interface IClientReadModelRepository
	{					
		AppointmentsOfADayReadModel GetAppointmentsOfADayReadModel(AggregateIdentifier id);
		bool IsAppointmentsOfADayReadModelAvailable(AggregateIdentifier id);
		void RequestAppointmentsOfADayReadModel(Action<AppointmentsOfADayReadModel> readModelAvailable, 
												AggregateIdentifier id, Action<string> errorCallback);
	}
}