using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage
{
	internal interface IUserPageViewModel : IViewModel
    {
        ICommand AddUser     { get; }
		ICommand SaveChanges { get; }

		ObservableCollection<User> Users { get; } 

		User SelectedUser { get; set; }

		bool ShowModificationView { get; }
    }
}