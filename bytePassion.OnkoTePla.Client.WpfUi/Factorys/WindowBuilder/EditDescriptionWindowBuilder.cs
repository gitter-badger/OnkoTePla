using System;
using System.Net.Mime;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Views;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
    internal class EditDescriptionWindowBuilder : IWindowBuilder<EditDescription>
    {
        private readonly Appointment appointmentToEdit;
        private IViewModelCommunication viewModelCommunication;
        private readonly ISharedState<AppointmentModifications> modificationsVar;
        private readonly Guid practiseId;

        public EditDescriptionWindowBuilder( Appointment appointmentToEdit)
        {
            this.appointmentToEdit = appointmentToEdit;
        }

        public EditDescriptionWindowBuilder(Appointment appointmentToEdit, IViewModelCommunication viewModelCommunication, ISharedState<ViewModels.AppointmentView.Helper.AppointmentModifications> modificationsVar ,Guid practiseId) : this(appointmentToEdit)
        {
            this.viewModelCommunication = viewModelCommunication;
            this.modificationsVar = modificationsVar;
            this.practiseId = practiseId;
        }

        public EditDescription BuildWindow()
        {
            var view = new EditDescription();
            view.DataContext = new EditDescriptionViewModel(appointmentToEdit, viewModelCommunication, modificationsVar, practiseId);
            view.Owner = Application.Current.MainWindow;
            return view;
        }

        public void DisposeWindow(EditDescription buildedWindow)
        {
            
        }
    }
}