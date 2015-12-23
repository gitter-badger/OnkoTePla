using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Data;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage
{
	internal class UserPageViewModel : ViewModel, 
									   IUserPageViewModel
    {
	    private readonly IDataCenter dataCenter;
		private User selectedUser;
		private bool showModificationView;

		public UserPageViewModel(IDataCenter dataCenter)
	    {
		    this.dataCenter = dataCenter;	
			
			AddUser        = new Command(DoAddUser);	
			SaveChanges    = new Command(DoSaveChanges);
			DiscardChanges = new Command(DoDiscardChanges);

		    Users = dataCenter.GetAllUsers()
							  .ToObservableCollection();

			ShowModificationView = false;
	    }

		private void DoDiscardChanges()
		{
			SelectedUser = null;
			ShowModificationView = false;
		}

		private void DoSaveChanges()
	    {
		    SelectedUser = null;
			ShowModificationView = false;
		}

	    private void DoAddUser()
	    {
		    var newUser = UserCreateAndUpdateLogic.Create();
		    dataCenter.AddNewUser(newUser);
			Users.Add(newUser);
	    }

	    public ICommand AddUser        { get; }
		public ICommand SaveChanges    { get; }
		public ICommand DiscardChanges { get; }

		public ObservableCollection<User> Users { get; }

		public User SelectedUser
		{
			get { return selectedUser; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref selectedUser, value);

				ShowModificationView = SelectedUser != null;
			}
		}

		public bool ShowModificationView
		{
			get { return showModificationView; }
			private set { PropertyChanged.ChangeAndNotify(this, ref showModificationView, value); }
		}

		protected override void CleanUp() {  }
        public override event PropertyChangedEventHandler PropertyChanged;	    
    }
}
