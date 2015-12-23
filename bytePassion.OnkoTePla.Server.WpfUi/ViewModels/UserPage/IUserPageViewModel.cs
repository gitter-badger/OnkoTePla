using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage
{
	internal interface IUserPageViewModel : IViewModel
    {
        ICommand AddUser        { get; }
		ICommand SaveChanges    { get; }
		ICommand DiscardChanges { get; }    

		ObservableCollection<User> Users { get; } 

		User SelectedUser { get; set; }

		bool ShowModificationView { get; }

		string UserName { get; set; }
		string Password { get; set; }
		bool   IsHidden { get; set; }
    }
}