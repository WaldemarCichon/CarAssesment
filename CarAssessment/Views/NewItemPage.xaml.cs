using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CarAssessment.Components;

using CarAssessment.Models;
using CarAssessment.ViewModels;
using CarAssessment.Models.Row;
using CarAssessment.Services;
using System.Threading.Tasks;
using CarAssessment.Models.Collection;
using CarAssessment.Layout;

// Page is used as well for the new item but also for the editing
namespace CarAssessment.Views {
	public partial class NewItemPage : ContentPage {
        private LayoutController layoutController;

        public Item Item { get; set; }
		class Damage {
			String x;
			int y;
			String z;
		}

		IDataStore<Assessment> DataStore => DependencyService.Get<IDataStore<Assessment>>();
		Assessment Assessment { get; set; }

		public NewItemPage(): this(null) {

		}

		public NewItemPage(Assessment assessment) {
			InitializeComponent();
			InitializeContext(assessment);
			layoutController = new LayoutController(this);
		}

		private async void InitializeContext(Assessment assessment) {
			// BindingContext = new NewItemViewModel();
			if (assessment == null) {
				assessment = await DataStore.CreateItemAsync();
			}
			Assessment = assessment;
			BindingContext = assessment;
			DamagePhotos.SourceList = new ImageList(assessment.DamagePhotos);
			NearbyPhotos.SourceList = new ImageList(assessment.NearbyPhotos);
			DetailPhotos.SourceList = new ImageList(assessment.DetailPhotos);
			OtherPhotos.SourceList = new ImageList(assessment.OtherPhotos);

			FrontLeftPhoto.ImagePath = assessment.FrontLeftPhotoPath;
			FrontRightPhoto.ImagePath = assessment.FrontRightPhotoPath;
			RearLeftPhoto.ImagePath = assessment.RearLeftPhotoPath;
			RearRightPhoto.ImagePath = assessment.RearRightPhotoPath;

			var l = new List<Damage>();
			l.Add(new Damage());
			l.Add(new Damage());
			DamageListView.ItemsSource = l;
			PreDamage.ItemsSource = l;
		}

		async void AddNewPreDamageImage_Clicked(System.Object sender, System.EventArgs e) {
			await Shell.Current.GoToAsync(nameof(CameraPage));
		}

		void CancelButton_Clicked(System.Object sender, System.EventArgs e) {
		}

		void SaveButton_Clicked(System.Object sender, System.EventArgs e) {
			DataStore.UpdateItemAsync(Assessment);	
		}

	}
}
