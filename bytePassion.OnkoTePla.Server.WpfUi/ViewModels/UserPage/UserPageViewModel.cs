using System.ComponentModel;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage
{
    internal class UserPageViewModel : ViewModel, IUserPageViewModel
    {
        protected override void CleanUp() {  }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
