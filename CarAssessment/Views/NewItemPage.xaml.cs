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
        private LayoutController LayoutController { get; set; }

        public Item Item { get; set; }
		class Damage {
			String x;
			int y;
			String z;
		}

		public Command TitleClicked { get; } = new Command((param) => Console.WriteLine(param));

		IDataStore<Assessment> DataStore => DependencyService.Get<IDataStore<Assessment>>();
		Assessment Assessment { get; set; }

		public NewItemPage(): this(null) {

		}

		public NewItemPage(Assessment assessment) {
			InitializeComponent();
			InitializeContext(assessment);
			LayoutController = new LayoutController(this);
			var grid = this.Content as Grid;
			var stackLayout = grid.Children[0] as StackLayout;
			foreach (var view in stackLayout.Children) {
				if (view.GetType() == typeof(Label)) {
					var label = view as Label;
					if (label.AutomationId == "0") {
						if (label.GestureRecognizers.Count > 0) {
							var tapGestureRecognizer = label.GestureRecognizers[0] as TapGestureRecognizer;
							tapGestureRecognizer.Command = TitleClicked;
						}
					}
				}
			}
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
		}

		async void AddNewPreDamageImage_Clicked(System.Object sender, System.EventArgs e) {
			await Shell.Current.GoToAsync(nameof(CameraPage));
		}

		void CancelButton_Clicked(System.Object sender, System.EventArgs e) {
		}

		internal void HandleSpecialFields(int displayedGroup) {
			if (displayedGroup == 0) {
				NavigationButtons.IsVisible = false;
				DialogBox.IsVisible = false;
			} else {
				NavigationButtons.IsVisible = true;
				DialogBox.IsVisible = true;
			}
			if (displayedGroup == 7) {
				PoliceReport.IsVisible = Assessment.IsPoliceReport == true;
				RecommendedAdvocate.IsVisible = Assessment.WantAdvocate == true && Assessment.IsRecommendedAdvocate==true;
				CustomersAdvocate.IsVisible = Assessment.WantAdvocate == true && Assessment.IsRecommendedAdvocate == false;
			}
		}

		internal void HandleSpecialFields(object sender, CheckedChangedEventArgs args) {
			HandleSpecialFields(int.Parse(((View)sender).AutomationId));
		}

		void SaveButton_Clicked(System.Object sender, System.EventArgs e) {
			DataStore.UpdateItemAsync(Assessment);	
		}

		void Up(Object sender, EventArgs e) {
			LayoutController.Up();
		}

		void Previous(Object sender, EventArgs e) {
			LayoutController.Previous();
		}

		void Next(Object sender, EventArgs e) {
			LayoutController.Next();
		}

	}
}
