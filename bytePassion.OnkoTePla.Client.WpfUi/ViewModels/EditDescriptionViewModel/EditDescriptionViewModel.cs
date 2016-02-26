using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.EditDescriptionViewModel
{
	internal class EditDescriptionViewModel : ViewModel, IEditDescriptionViewModel
	{
        private string description;		
        private readonly ISharedState<AppointmentModifications> modificationsVar;        

        public EditDescriptionViewModel(Appointment appointmentToEdit, 										
										ISharedState<AppointmentModifications> modificationsVar)
        {	        
            this.modificationsVar = modificationsVar;            
            Description = appointmentToEdit.Description;
            Cancel = new Command(CloseWindow);
            Accept = new Command(SaveAndClose);
        }
		
        private void SaveAndClose()
        {
            modificationsVar.Value.SetNewDescription(Description);
            CloseWindow();

        }

        public ICommand Cancel { get; }
        public ICommand Accept { get; }

        public string Description
        {
            get { return description; }
            set { PropertyChanged.ChangeAndNotify(this, ref description, value); }
        }

        private static void CloseWindow()
        {
            var windows = Application.Current.Windows
                                             .OfType<Views.EditDescription>()
                                             .ToList();

            if (windows.Count == 1)
                windows[0].Close();
            else
                throw new Exception("inner error");
        }

		protected override void CleanUp() {}		
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
