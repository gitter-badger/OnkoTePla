using System;
using System.Windows;
using bytePassion.Lib.Communication.State;
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
        private readonly ISharedState<AppointmentModifications> modificationsVar;        
       
        public EditDescriptionWindowBuilder(Appointment appointmentToEdit, 
										    ISharedState<AppointmentModifications> modificationsVar) 			
        {            
            this.modificationsVar = modificationsVar;            
			this.appointmentToEdit = appointmentToEdit;
		}
		
        public EditDescription BuildWindow(Action<string> errorCallback)
        {
	        var view = new EditDescription
	        {
		        DataContext = new EditDescriptionViewModel(appointmentToEdit, modificationsVar),
		        Owner = Application.Current.MainWindow
	        };
	        return view;
        }

        public void DisposeWindow(EditDescription buildedWindow)
        {
			throw new NotImplementedException();
		}
    }
}