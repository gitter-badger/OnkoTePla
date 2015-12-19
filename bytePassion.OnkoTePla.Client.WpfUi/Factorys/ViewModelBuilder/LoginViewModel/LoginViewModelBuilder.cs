using System;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel
{
	internal class LoginViewModelBuilder : ILoginViewModelBuilder
    {
	    private readonly ISession session;

	    public LoginViewModelBuilder(ISession session)
	    {
		    this.session = session;
	    }

	    public ILoginViewModel Build()
        {
            return new ViewModels.LoginView.LoginViewModel(session);
        }

        public void DisposeViewModel(ILoginViewModel viewModelToDispose)
        {
            throw new NotImplementedException();
        }
    }

}
