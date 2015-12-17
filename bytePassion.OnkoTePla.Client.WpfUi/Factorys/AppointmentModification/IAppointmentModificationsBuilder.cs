using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using System;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.AppointmentModification
{
    internal interface IAppointmentModificationsBuilder
    {
        AppointmentModifications Build(Appointment originalAppointment, Guid medicalPracticeId, bool isInitialAdjustment);
    }
}
