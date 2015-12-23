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
		private string userName;
		private string password;
		private bool isHidden;

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
			var newUser = SelectedUser.SetNewUserValues(UserName,
														Password,
														SelectedUser.ListOfAccessableMedicalPractices,
														IsHidden);
			dataCenter.UpdateUser(newUser);

			Users.Remove(SelectedUser);
			Users.Add(newUser);

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
				var oldSelectedUser = SelectedUser;

				PropertyChanged.ChangeAndNotify(this, ref selectedUser, value);

				ShowModificationView = SelectedUser != null;

				if (SelectedUser != null && SelectedUser != oldSelectedUser)
				{
					UserName = SelectedUser.Name;
					Password = SelectedUser.Password;
					IsHidden = SelectedUser.IsHidden;
				}
			}
		}

		public bool ShowModificationView
		{
			get { return showModificationView; }
			private set { PropertyChanged.ChangeAndNotify(this, ref showModificationView, value); }
		}

		public string UserName
		{
			get { return userName; }
			set { PropertyChanged.ChangeAndNotify(this, ref userName, value); }
		}

		public string Password
		{
			get { return password; }
			set { PropertyChanged.ChangeAndNotify(this, ref password, value); }
		}

		public bool IsHidden
		{
			get { return isHidden; }
			set { PropertyChanged.ChangeAndNotify(this, ref isHidden, value); }
		}

		protected override void CleanUp() {  }
        public override event PropertyChangedEventHandler PropertyChanged;	    
    }
}
