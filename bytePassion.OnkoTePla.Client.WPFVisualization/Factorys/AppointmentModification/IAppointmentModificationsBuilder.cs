using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using System;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.AppointmentModification
{
    public interface IAppointmentModificationsBuilder
    {
        AppointmentModifications Build(Appointment originalAppointment, Guid medicalPracticeId, bool isInitialAdjustment);
    }
}
