using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage.Helper;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage
{
	internal interface ITherapyPlaceTypesPageViewModel : IViewModel
	{
		ICommand AddTherapyPlaceType { get; }
		ICommand SaveChanges         { get; }
		ICommand DiscardChanges      { get; }

		ObservableCollection<TherapyPlaceType> TherapyPlaceTypes { get; }

		TherapyPlaceType SelectedTherapyPlaceType { get; set; }
		
		bool ShowModificationView { get; }

		string          Name     { get; set; }
		IconDisplayData IconType { get; set; }

		ObservableCollection<IconDisplayData> AllIcons { get; }
	}
}