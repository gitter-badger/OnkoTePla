using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel
{
    class EditDescriptionViewModel : IViewModel
    {
        private string description;
        private Appointment appointment;

        public EditDescriptionViewModel(Appointment appointmentToEdit)
        {
            appointment = appointmentToEdit;
            description = appointment.Description;
            Cancel = new Command(CloseWindow);
            Accept = new Command(SaveAndClose);
        }

        private void SaveAndClose()
        {
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
