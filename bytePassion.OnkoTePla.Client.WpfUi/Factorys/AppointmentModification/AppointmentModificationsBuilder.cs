using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Model;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using System;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.AppointmentModification
{
    public class AppointmentModificationsBuilder : IAppointmentModificationsBuilder
    {
        private readonly IDataCenter dataCenter;
        private readonly IViewModelCommunication viewModelCommunication;
        private readonly IGlobalState<Date> selectedDateVariable;
        private readonly IGlobalStateReadOnly<Size> gridSizeVariable;

        public AppointmentModificationsBuilder(IDataCenter dataCenter,
                                               IViewModelCommunication viewModelCommunication,
                                               IGlobalState<Date> selectedDateVariable,
                                               IGlobalStateReadOnly<Size> gridSizeVariable)
        {
            this.dataCenter = dataCenter;
            this.viewModelCommunication = viewModelCommunication;
            this.selectedDateVariable = selectedDateVariable;
            this.gridSizeVariable = gridSizeVariable;
        }

        public AppointmentModifications Build(Appointment originalAppointment, Guid medicalPracticeId, bool isInitialAdjustment)
        {
            return new AppointmentModifications(originalAppointment,
                                                medicalPracticeId,
                                                dataCenter,
                                                viewModelCommunication,
                                                selectedDateVariable,
                                                gridSizeVariable,
                                                isInitialAdjustment);
        }
    }
}