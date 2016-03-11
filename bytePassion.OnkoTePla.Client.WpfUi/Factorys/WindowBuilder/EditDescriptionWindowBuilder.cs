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
		private readonly Action<string> errorCallback;

		public EditDescriptionWindowBuilder(Appointment appointmentToEdit, 
										    ISharedState<AppointmentModifications> modificationsVar,
											Action<string> errorCallback) 			
        {            
            this.modificationsVar = modificationsVar;
			this.errorCallback = errorCallback;
			this.appointmentToEdit = appointmentToEdit;
		}
		
        public EditDescription BuildWindow()
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