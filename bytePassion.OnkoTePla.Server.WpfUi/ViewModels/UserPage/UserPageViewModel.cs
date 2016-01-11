using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage.Helper;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage
{
	internal class UserPageViewModel : ViewModel, 
									   IUserPageViewModel
    {
	    private readonly IDataCenter dataCenter;
		private readonly IGlobalStateReadOnly<MainPage> selectedPageVariable;		

		private User selectedUser;
		private bool showModificationView;
		private string userName;
		private string password;
		private bool isHidden;

		public UserPageViewModel(IDataCenter dataCenter, 
								 IGlobalStateReadOnly<MainPage> selectedPageVariable)
	    {
		    this.dataCenter = dataCenter;
			this.selectedPageVariable = selectedPageVariable;

			AddUser        = new Command(DoAddUser);	
			SaveChanges    = new Command(DoSaveChanges);
			DiscardChanges = new Command(DoDiscardChanges);

		    Users = dataCenter.GetAllUsers()
							  .ToObservableCollection();

			AccessablePractices = new ObservableCollection<MedPracticeListItemData>();

			ShowModificationView = false;
			
			selectedPageVariable.StateChanged += OnSelectedPageChanged;
	    }

		private void OnSelectedPageChanged(MainPage mainPage)
		{
			if (mainPage != MainPage.User)
			{
				SelectedUser = null;
			}			
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
														AccessablePractices.Where(listItem => listItem.IsSelected)
																		   .Select(listItem => listItem.Id)
																		   .ToList(),
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

		    SelectedUser = newUser;
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

					AccessablePractices.Clear();

					foreach (var medicalPractice in dataCenter.GetAllMedicalPractices())
					{
						AccessablePractices.Add(new MedPracticeListItemData(SelectedUser.ListOfAccessableMedicalPractices.Contains(medicalPractice.Id), 
																			medicalPractice.Name,
																			medicalPractice.Id));
					}
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

		public ObservableCollection<MedPracticeListItemData> AccessablePractices { get; }

		protected override void CleanUp()
		{
			selectedPageVariable.StateChanged -= OnSelectedPageChanged;
		}
        public override event PropertyChangedEventHandler PropertyChanged;	    
    }
}
