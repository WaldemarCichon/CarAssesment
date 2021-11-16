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
using CarAssessment.REST;
using System.IO;

// Page is used as well for the new item but also for the editing
namespace CarAssessment.Views {
	public partial class NewItemPage : ContentPage {
		private static LayoutController LayoutController { get; set; }
		private static BoxView dialogOuterBox {get;set;}

        public Item Item { get; set; }

		public Command TitleClicked { get; } = new Command((param) => {
			LayoutController.DisplayedGroup = int.Parse(param as string);
			dialogOuterBox.IsVisible = true;
		});
			

		IDataStore<Assessment> DataStore => DependencyService.Get<IDataStore<Assessment>>();
		Assessment Assessment { get; set; }
		public static Assessment CurrentAssessment { get; internal set; }

		public NewItemPage(): this(null) {

		}

		public NewItemPage(Assessment assessment) {
			InitializeComponent();
			InitializeContext(assessment);
			dialogOuterBox = DialogOuterBox;
			LayoutController = new LayoutController(this);
			PrevArrowButton.BindingContext = LayoutController;
			NextArrowButton.BindingContext = LayoutController;
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
			CurrentAssessment = assessment;
			BindingContext = assessment;

			DamagePhotos.SourceList = new ImageList(assessment.DamagePhotos);
			NearbyPhotos.SourceList = new ImageList(assessment.NearbyPhotos);
			DetailPhotos.SourceList = new ImageList(assessment.DetailPhotos);
			OtherPhotos.SourceList = new ImageList(assessment.OtherPhotos);
			Device.BeginInvokeOnMainThread(() => {
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				FrontLeftPhoto.ImagePath = assessment.FrontLeftPhotoPath;
				FrontRightPhoto.ImagePath = assessment.FrontRightPhotoPath;
				RearLeftPhoto.ImagePath = assessment.RearLeftPhotoPath;
				RearRightPhoto.ImagePath = assessment.RearRightPhotoPath;
				CarDocument.ImagePath = assessment.CarDocumentPhotoPath;
				ServiceRecordBook.ImagePath = assessment.ServiceRecordBookPhotoPath;
				PoliceReport.ImagePath = assessment.PoliceReportPhotoPath;
				SpeedometerPhoto.ImagePath = assessment.SpeedometerPhotoPath;
				ChassisNumberPhoto.ImagePath = assessment.ChassisNumberPhotoPath;

				DamageDescriptions.ItemsSource = assessment.DamageDescriptions;
				PreDamageDescriptions.ItemsSource = assessment.PreDamages;
			});
			
			foreach (var preDamage in assessment.PreDamages) {
				var imagePath = preDamage.ImagePath;
				if (imagePath == null || imagePath == "") {
					continue;
				}
				imagePath = Path.GetFileName(imagePath);
				preDamage.ImagePath = imagePath;
			}
		}

		async void AddNewPreDamageImage_Clicked(System.Object sender, System.EventArgs e) {
			await Shell.Current.GoToAsync(nameof(CameraPage));
		}

		protected override bool OnBackButtonPressed() {
			return DisplayAlert("Abbrechen?", "Wollen Sie die Eingabe wirklich ohne Speichern abbrechen?", "Ja", "Nein").Result;
		}

		async void CancelButton_Clicked(System.Object sender, System.EventArgs e) {
			if (await DisplayAlert("Abbrechen?", "Wollen Sie die Eingabe wirklich ohne Speichern abbrechen?", "Ja", "Nein")) {
				await Shell.Current.GoToAsync("..");
			}
		}

		internal void HandleSpecialFields(int displayedGroup) {
			if (displayedGroup == 0) {
				NavigationButtons.IsVisible = false;
				DialogBox.IsVisible = false;
			} else {
				NavigationButtons.IsVisible = true;
				DialogBox.IsVisible = true;
			}
			if (displayedGroup == 9) {
				PoliceReport.IsVisible = Assessment.IsPoliceReport == true;
				RecommendedAdvocate.IsVisible = Assessment.WantAdvocate == true && Assessment.IsRecommendedAdvocate==true;
				CustomersAdvocate.IsVisible = Assessment.WantAdvocate == true && Assessment.IsRecommendedAdvocate == false;
			}
		}

		internal void HandleSpecialFields(object sender, CheckedChangedEventArgs args) {
			HandleSpecialFields(int.Parse(((View)sender).AutomationId));
		}

		async void checkAndPersistSignature() {
			if (Signature.IsBlank) {
				return;
			}
			var signatureImgStream = await Signature.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Jpeg);
			using (MemoryStream ms = new MemoryStream()) {
				await (signatureImgStream as Stream).CopyToAsync(ms);
				var bytes = ms.ToArray();
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				var path = Path.Combine(documents, $"signature_{Assessment.Id}");
				System.IO.File.WriteAllBytes(path, bytes);
				await HttpRepository.Instance.PostPicture(path);
			}
		}

		async void SaveButton_Clicked(System.Object sender, System.EventArgs e) {
			checkAndPersistSignature();
			foreach (var preDamage in Assessment.PreDamages) {
				if (preDamage.TempImagePath != null) {
					preDamage.ImagePath = preDamage.TempImagePath;
				}
			}
			if (Assessment.IsNewRow) {
				Assessment.LastSaved = DateTime.Now;
				await DataStore.AddItemAsync(Assessment);
			} else {
				await DataStore.UpdateItemAsync(Assessment);
			}

			await DataStore.Commit();
			
			if (await DisplayAlert("Senden", "Soll der Datensatz auch gesendet werden?", "Ja", "Nein")) { 
				if (Assessment.ObjectId < 1) {
					await HttpRepository.Instance.PostAssessment(Assessment);
					await DataStore.UpdateItemAsync(Assessment);
				} else {
					await HttpRepository.Instance.PutAssessment(Assessment);
				}
			}
			await Shell.Current.GoToAsync("..");
		}

		void Up(Object sender, EventArgs e) {
			LayoutController.Up();
			DialogOuterBox.IsVisible = false;
		}

		void Previous(Object sender, EventArgs e) {
			LayoutController.Previous();
		}

		void Next(Object sender, EventArgs e) {
			LayoutController.Next();
		}

		void NewDamageDescriptionButton_Clicked(System.Object sender, System.EventArgs e) {
			Assessment.DamageDescriptions.Add(new DamageDescription());
			DamageDescriptions.ItemsSource = null;
			DamageDescriptions.ItemsSource = Assessment.DamageDescriptions;
		}

		void NewPreDamageDescriptionButton_Clicked(System.Object sender, System.EventArgs e) {
			Assessment.PreDamages.Add(new PreDamage());
			PreDamageDescriptions.ItemsSource = null;
			PreDamageDescriptions.ItemsSource = Assessment.PreDamages;
		}

		async void DeleteDamageDescriptionButton_Clicked(System.Object sender, System.EventArgs e) {
			if (!await DisplayAlert("Schaden löschen", "Wollen Sie die Beschreibung des Schadens löschen?", "Ja", "Nein")) {
				return;
			}
			var damageDescription = (sender as Button).CommandParameter as DamageDescription;
			Assessment.DamageDescriptions.Remove(damageDescription);
			DamageDescriptions.ItemsSource = null;
			DamageDescriptions.ItemsSource = Assessment.DamageDescriptions;
		}

		async void DeletePreDamageDescriptionButton_Clicked(System.Object sender, System.EventArgs e) { 
			if (!await DisplayAlert("Vorschaden löschen","Wollen Sie die Beschreibung des Vorschaden löschen?","Ja","Nein")) {
				return;
			}
			var preDamageDescription = (sender as Button).CommandParameter as PreDamage;
			Assessment.PreDamages.Remove(preDamageDescription);
			PreDamageDescriptions.ItemsSource = null;
			PreDamageDescriptions.ItemsSource = Assessment.PreDamages;
		}

		void SignaturePadView_Cleared(System.Object sender, System.EventArgs e) {
			
		}
	}
}
