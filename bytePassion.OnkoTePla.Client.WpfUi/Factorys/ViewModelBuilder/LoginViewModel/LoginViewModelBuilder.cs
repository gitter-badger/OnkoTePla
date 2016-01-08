using System;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel
{
	internal class LoginViewModelBuilder : ILoginViewModelBuilder
    {
	    private readonly ISession session;
		private readonly IDataCenter dataCenter;

		public LoginViewModelBuilder(ISession session, 
									 IDataCenter dataCenter)
		{
			this.session = session;
			this.dataCenter = dataCenter;
		}
		
		public ILoginViewModel Build()
        {
            return new ViewModels.LoginView.LoginViewModel(session, dataCenter);
        }

        public void DisposeViewModel(ILoginViewModel viewModelToDispose)
        {
            throw new NotImplementedException();
        }
    }

}
