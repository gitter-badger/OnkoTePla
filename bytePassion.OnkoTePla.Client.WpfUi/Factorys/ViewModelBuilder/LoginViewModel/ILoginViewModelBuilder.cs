using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel
{
    internal interface ILoginViewModelBuilder
    {
        ILoginViewModel Build();
        void DisposeViewModel(ILoginViewModel viewModelToDispose);
    }
}