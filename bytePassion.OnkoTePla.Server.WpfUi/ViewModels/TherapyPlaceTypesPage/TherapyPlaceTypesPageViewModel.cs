using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Utils;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Contracts.Enums;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage.Helper;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage
{
	internal class TherapyPlaceTypesPageViewModel : ViewModel, 
													ITherapyPlaceTypesPageViewModel
	{
		private const string BasePath = "pack://application:,,,/bytePassion.OnkoTePla.Resources;component/Icons/TherapyPlaceType/";

		private readonly IDataCenter dataCenter;

		private TherapyPlaceType selectedTherapyPlaceType;
		private bool showModificationView;		
		private string name;
		private IconDisplayData iconType;

		public TherapyPlaceTypesPageViewModel (IDataCenter dataCenter)
		{
			this.dataCenter = dataCenter;

			AddTherapyPlaceType = new Command(DoAddTheraptPlaceType);
			SaveChanges         = new Command(DoSaveChanges);
			DiscardChanges      = new Command(DoDiscardChanges);

			TherapyPlaceTypes = dataCenter.GetAllTherapyPlaceTypes()
										  .ToObservableCollection();

			ShowModificationView = false;

			AllIcons = new ObservableCollection<IconDisplayData>
			{
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"bed01.png")),   TherapyPlaceIconType.BedType1,   "Bed1"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"bed02.png")),   TherapyPlaceIconType.BedType2,   "Bed2"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"bed03.png")),   TherapyPlaceIconType.BedType3,   "Bed3"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"bed04.png")),   TherapyPlaceIconType.BedType4,   "Bed4"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"bed05.png")),   TherapyPlaceIconType.BedType5,   "Bed5"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"chair01.png")), TherapyPlaceIconType.ChairType1, "Chair1"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"chair02.png")), TherapyPlaceIconType.ChairType2, "Chair2"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"chair03.png")), TherapyPlaceIconType.ChairType3, "Chair3"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"chair04.png")), TherapyPlaceIconType.ChairType4, "Chair4"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"chair05.png")), TherapyPlaceIconType.ChairType5, "Chair5"),
				new IconDisplayData(ImageLoader.LoadImage(new Uri(BasePath +"none.png")),    TherapyPlaceIconType.None,       "None"),
			};
		}

		private void DoDiscardChanges ()
		{
			SelectedTherapyPlaceType = null;
			ShowModificationView = false;
		}

		private void DoSaveChanges ()
		{
			var newTherapyPlaceType = SelectedTherapyPlaceType.SetNewName(Name)
															  .SetNewIcon(IconType.IconType);
			dataCenter.UpdateTherapyPlaceType(newTherapyPlaceType);

			TherapyPlaceTypes.Remove(SelectedTherapyPlaceType);
			TherapyPlaceTypes.Add(newTherapyPlaceType);

			SelectedTherapyPlaceType = null;
			ShowModificationView = false;
		}

		private void DoAddTheraptPlaceType ()
		{
			var newTherapyPlaceType = TherapyPlaceTypeCreateAndEditLogic.Create();
			dataCenter.AddNewTherapyPlaceType(newTherapyPlaceType);
			TherapyPlaceTypes.Add(newTherapyPlaceType);

			SelectedTherapyPlaceType = newTherapyPlaceType;
		}
		
		public ICommand AddTherapyPlaceType { get; }
		public ICommand SaveChanges { get; }
		public ICommand DiscardChanges { get; }

		public ObservableCollection<TherapyPlaceType> TherapyPlaceTypes { get; }
				

		public TherapyPlaceType SelectedTherapyPlaceType
		{
			get { return selectedTherapyPlaceType; }
			set
			{
				var oldSelectedTherapyPlaceType = selectedTherapyPlaceType;

				PropertyChanged.ChangeAndNotify(this, ref selectedTherapyPlaceType, value);

				ShowModificationView = selectedTherapyPlaceType != null;

				if (selectedTherapyPlaceType != null && selectedTherapyPlaceType != oldSelectedTherapyPlaceType)
				{
					Name     = SelectedTherapyPlaceType.Name;
					IconType = AllIcons.First(icon => icon.IconType == SelectedTherapyPlaceType.IconType);
				}
			}
		}

		public bool ShowModificationView
		{
			get { return showModificationView; }
			private set { PropertyChanged.ChangeAndNotify(this, ref showModificationView, value); }
		}

		public string Name
		{
			get { return name; }
			set { PropertyChanged.ChangeAndNotify(this, ref name, value); }
		}

		public IconDisplayData IconType
		{
			get { return iconType; }
			set { PropertyChanged.ChangeAndNotify(this, ref iconType, value); }
		}

		public ObservableCollection<IconDisplayData> AllIcons { get; }

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}