using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel
{
    class EditDescriptionViewModel
    {
        public EditDescriptionViewModel(Appointment appointmentToEdit)
        {
            
            Description = appointmentToEdit.Description;
        }

        public string Description { get; set; }
    }
}
