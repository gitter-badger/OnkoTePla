using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Domain.Commands;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel
{
    class EditDescriptionViewModel : IViewModel
    {
        private string description;
        private Appointment appointment;
        private readonly IViewModelCommunication viewModelCommunication;
        private readonly IGlobalState<AppointmentModifications> modificationsVar;
        private readonly Guid practiseId;

        public EditDescriptionViewModel(Appointment appointmentToEdit, IViewModelCommunication viewModelCommunication, IGlobalState<ViewModels.AppointmentView.Helper.AppointmentModifications> modificationsVar, Guid practiseId
            )
        {
            appointment = appointmentToEdit;
            this.viewModelCommunication = viewModelCommunication;
            this.modificationsVar = modificationsVar;
            this.practiseId = practiseId;
            description = appointment.Description;
            Cancel = new Command(CloseWindow);
            Accept = new Command(SaveAndClose);
        }

        private void SaveAndClose()
        {
            modificationsVar.Value.SetNewDescription(Description);
            viewModelCommunication.SendTo(Constants.AppointmentGridViewModelCollection, new AggregateIdentifier(modificationsVar.Value.CurrentLocation.PlaceAndDate.Date,
                                            practiseId), new SendCurrentChangesToCommandBus());
            CloseWindow();

        }

        public ICommand Cancel { get; set; }
        public ICommand Accept { get; set; }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                }
            }
        }

        private void CloseWindow()
        {
            var windows = Application.Current.Windows
                                             .OfType<Views.EditDescription>()
                                             .ToList();

            if (windows.Count == 1)
                windows[0].Close();
            else
                throw new Exception("inner error");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            
        }
    }
}
