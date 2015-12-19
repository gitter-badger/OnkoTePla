using System.ComponentModel;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView
{
	internal class LoginViewModel : ViewModel, 
                                    ILoginViewModel
    {
	    private readonly ISession session;

	    public LoginViewModel(ISession session)
	    {
		    this.session = session;
	    }

	    protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
