﻿using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder
{
	public interface IAppointmentGridViewModelBuilder
	{
		IAppointmentGridViewModel Build(AggregateIdentifier identifier);
	}
}