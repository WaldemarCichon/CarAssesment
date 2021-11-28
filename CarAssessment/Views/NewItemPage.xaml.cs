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
using System.Resources;
using CarAssessment.Tooling;
using SignaturePad.Forms;

[assembly: NeutralResourcesLanguage("de-DE")]
// Page is used as well for the new item but also for the editing
namespace CarAssessment.Views {
	public partial class NewItemPage : ContentPage {
		private static LayoutController LayoutController { get; set; }
		private static BoxView dialogOuterBox {get;set;}

		IDataStore<Assessment> DataStore => DependencyService.Get<IDataStore<Assessment>>();
		Assessment Assessment { get; set; }
		public static Assessment CurrentAssessment { get; internal set; }

		private TitledEntryField[] numericFieldsToUpdate;
		private bool newAssessment = false;

		public NewItemPage(): this(null) {
			this.newAssessment = true;
		}

		public NewItemPage(Assessment assessment) {
			InitializeComponent();
			InitializeComponentInternal();
			InitializeContext(assessment);
			dialogOuterBox = DialogOuterBox;
			LayoutController = new LayoutController(this, newAssessment);
			//PrevArrowButton.BindingContext = LayoutController;
			//NextArrowButton.BindingContext = LayoutController;
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

		private void InitializeComponentInternal() {
			numericFieldsToUpdate = new TitledEntryField[] {
				HourlyRateBodyField,
				HourlyRateElectricField,
				UPESurchargeField,
				SmallPartsSurchargeField,
				FlatRatePaintingField,
				HourlyRatePaintingSurchargeField,
				HourlyRatePaintPoinField,
				TimedRateTransportField,
				FlatrateTransportField,
				MileageField
			};
			DeclarationOfAssignmentLabel.Text = TextTemplates.DeclarationOfAssignmentText;
			AdvocateAssignmentLabel.Text = TextTemplates.AdvocateAssignmentText;
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

			fillSignature(Signature, "assignment");
			fillSignature(Signature1, "advocate");
		}

		public void fillSignature(SignaturePadView signature, string kind) {
			
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(documents, $"signature_{kind}_{Assessment.Id}.jpg");
			if (File.Exists(path)) {
				signature.BackgroundImage = path;
				signature.IsEnabled = false;
			}
		}

		public Command TitleClicked { get; } = new Command((param) => {
			int group = int.Parse(param as string);

			LayoutController.DisplayedGroup = group;

			Device.BeginInvokeOnMainThread(() => {
				dialogOuterBox.IsVisible = true;
			});
		});

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

		async void checkAndPersistSignature(SignaturePadView signature, string kind) {
			if (signature.IsBlank) {
				return;
			}
			var signatureImgStream = await signature.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Jpeg);
			var mmm = new MemoryStream();
			using (MemoryStream ms = new MemoryStream()) {
				await (signatureImgStream as Stream).CopyToAsync(ms);
				var bytes = ms.ToArray();
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				var path = Path.Combine(documents, $"signature_{kind}_{Assessment.Id}.jpg");
				File.WriteAllBytes(path, bytes);
				await HttpRepository.Instance.PostPicture(path);
			}
		}

		private void workAroundForDecimalComma() {
			foreach(var field in numericFieldsToUpdate) {
				var text = field.Text;
				text = text.Replace(".", "");
				text = text.Replace(',', '.');
				field.Text = text;
			}
		}

		async void SaveButton_Clicked(System.Object sender, System.EventArgs e) {
			checkAndPersistSignature(Signature, "assignment");
			checkAndPersistSignature(Signature1, "advocate");
			workAroundForDecimalComma();
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
			
			if (await DisplayAlert("Senden", "Soll der Datensatz auch gesendet werden?", "Ja", "Nein")) { 
				if (Assessment.ObjectId < 1) {
					await HttpRepository.Instance.PostAssessment(Assessment);
				} else {
					await HttpRepository.Instance.PutAssessment(Assessment);
				}
				await DataStore.UpdateItemAsync(Assessment);
			}
			await DataStore.Commit();
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

		public void EnterDeclarationOfAssignment() {
			Owner.Text = Assessment.OwnerName;
			PlateOwner.Text = Assessment.LicensePlateClient;
			PlateOpponent.Text = Assessment.LicensePlateOponent;
			Address.Text = Assessment.Street + "\n" + Assessment.City;
			AdmissionDate.Text = Assessment.AdmissionDate.ToString("dd.MM.yyyy");
			AccidentDate.Text = Assessment.AccidentDate.ToString("dd.MM.yyyy");
			CityAndDate.Text = Assessment.City + ", den " + Assessment.AdmissionDate.ToString("dd.MM.yy");
		}

		public void EnterAdvocateAssignment() {
			Accident.Text = "Schadenfall vom " + Assessment.AccidentDate.ToString("dd.MM.yyyy") + ", Kfz-Kennzeichen: " + Assessment.LicensePlateClient;
			Advocate.Text = "Rechtsanwalt: "+ Assessment.RecommendedAdvocate;
			Advocate1.Text = "dem Rechtsanwalt: " + Assessment.RecommendedAdvocate;
			Client.Text = "Hiermit erteile ich " + Assessment.OwnerName + " " + Assessment.Street + ", " + Assessment.City;
			CityAndDate1.Text = Assessment.City + ", den " + Assessment.AdmissionDate.ToString("dd.MM.yy");
		}
	}
}
