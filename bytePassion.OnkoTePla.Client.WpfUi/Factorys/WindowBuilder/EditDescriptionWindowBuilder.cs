using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Views;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
    internal class EditDescriptionWindowBuilder : IWindowBuilder<EditDescription>
    {
        private readonly Appointment appointmentToEdit;

        public EditDescriptionWindowBuilder( Appointment appointmentToEdit)
        {
            this.appointmentToEdit = appointmentToEdit;
        }

        public EditDescription BuildWindow()
        {
            var view = new EditDescription();
            view.DataContext = new EditDescriptionViewModel(appointmentToEdit);
            return view;
        }

        public void DisposeWindow(EditDescription buildedWindow)
        {
            
        }
    }
}