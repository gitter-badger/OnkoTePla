using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Contracts.Enums;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage.Helper;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage
{
	internal class TherapyPlaceTypesPageViewModelSampleData : ITherapyPlaceTypesPageViewModel
	{
		private const string BasePath = "pack://application:,,,/bytePassion.OnkoTePla.Resources;component/Icons/TherapyPlaceType/";
			 

		public TherapyPlaceTypesPageViewModelSampleData()
		{
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

			TherapyPlaceTypes = new ObservableCollection<TherapyPlaceType>
			{
				new TherapyPlaceType("example1", TherapyPlaceIconType.BedType2,   Guid.NewGuid()),
				new TherapyPlaceType("example2", TherapyPlaceIconType.ChairType1, Guid.NewGuid())
			};

			SelectedTherapyPlaceType = TherapyPlaceTypes.First();

			Name     = SelectedTherapyPlaceType.Name;
			IconType = AllIcons[1];

			ShowModificationView = true;			
		}

		public ICommand AddTherapyPlaceType => null;
		public ICommand SaveChanges         => null;
		public ICommand DiscardChanges      => null;

		public ObservableCollection<TherapyPlaceType> TherapyPlaceTypes { get; }

		public TherapyPlaceType SelectedTherapyPlaceType { get; set; }

		public bool ShowModificationView { get; }

		public string          Name     { get; set; }
		public IconDisplayData IconType { get; set; }

		public ObservableCollection<IconDisplayData> AllIcons { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}