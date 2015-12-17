using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;
using System;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel
{
    internal class LoginViewModelBuilder : ILoginViewModelBuilder
    {
        public ILoginViewModel Build()
        {
            return new ViewModels.LoginView.LoginViewModel();
        }

        public void DisposeViewModel(ILoginViewModel viewModelToDispose)
        {
            throw new NotImplementedException();
        }
    }

}
