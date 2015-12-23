using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Data;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage
{
	internal class UserPageViewModel : ViewModel, IUserPageViewModel
    {
	    private readonly IDataCenter dataCenter;
		
	    public UserPageViewModel(IDataCenter dataCenter)
	    {
		    this.dataCenter = dataCenter;	
			
			AddUser     = new Command(DoAddUser);	
			SaveChanges = new Command(DoSaveChanges);

		    Users = dataCenter.GetAllUsers()
							  .ToObservableCollection();
	    }
		
	    private void DoSaveChanges()
	    {		    
	    }

	    private void DoAddUser()
	    {
		    
	    }

	    public ICommand AddUser     { get; }
		public ICommand SaveChanges { get; }

		public ObservableCollection<User> Users { get; }

		public User SelectedUser { get; set; }

		protected override void CleanUp() {  }
        public override event PropertyChangedEventHandler PropertyChanged;	    
    }
}
