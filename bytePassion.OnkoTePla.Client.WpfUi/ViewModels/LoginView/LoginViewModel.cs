using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView
{
    internal class LoginViewModel : ViewModel, 
                                    ILoginViewModel
    {
        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
