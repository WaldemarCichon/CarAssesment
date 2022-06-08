using System;
using CarAssessment.Components;

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
using Xamarin.Forms;
using CarAssessment.Views.AbstractViews;
using System.Threading;
using Acr.UserDialogs;

[assembly: NeutralResourcesLanguage("de-DE")]
// Page is used as well for the new item but also for the editiƒadng
namespace CarAssessment.Views {
	public partial class NewItemPage : NewItemPageBase {
		private static LayoutController LayoutController { get; set; }
		private static BoxView dialogOuterBox {get;set;}
		private static ScrollView mainScrollView { get; set; }

		IDataStore<Assessment> DataStore => DependencyService.Get<IDataStore<Assessment>>();
		Assessment Assessment { get; set; }
		public static Assessment CurrentAssessment { get; internal set; }
		public static CreationMode CreationMode { get; internal set; }

		public const string AssignmentLetter = "assignment";
		public const string AdvocateLetter = "advocate";
		private TitledEntryField[] numericFieldsToUpdate;
		private bool newAssessment = false;

		public NewItemPage(): this(null) {
		}

		public NewItemPage(Assessment assessment) {
			InitializeComponent();
			InitializeComponentInternal();
			InitializeContext(assessment);
			dialogOuterBox = DialogOuterBox;
			LayoutController = new LayoutController(this, CreationMode);
			//PrevArrowButton.BindingContext = LayoutController;
			//NextArrowButton.BindingContext = LayoutController;
			var grid = this.Content as Grid;
			var stackLayout = TitleScrollLayout;
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
				MileageField,
				FrontLeftTireThreadDepth,
				FrontRightTireThreadDepth,
				RearLeftTireThreadDepth,
				RearRightTireThreadDepth
			};
			DeclarationOfAssignmentLabel.Text = TextTemplates.DeclarationOfAssignmentText;
			AdvocateAssignmentLabel.Text = TextTemplates.AdvocateAssignmentText;
			DialogOuterBox.IsVisible = false;
			mainScrollView = MainScrollView;

			BackButton.Command = new Command(async () => await GoBack());
		}

		private async Task GoBack() {
			var result = await Shell.Current.DisplayAlert(
				"Verlassen?",
				"Wollen Sie wirklich die Eingabe verlasen? All Ihre neu eingegebenen Daten werden nicht gespeichert?",
				"Ja!", "Nein!");

			if (result) {
				await Shell.Current.GoToAsync("..", true);
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
			ServiceRecordBookPhotos.SourceList = new ImageList(assessment.ServiceRecordBookPhotos);
			Device.BeginInvokeOnMainThread(() => {
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				FrontLeftPhoto.ImagePath = assessment.FrontLeftPhotoPath;
				FrontRightPhoto.ImagePath = assessment.FrontRightPhotoPath;
				RearLeftPhoto.ImagePath = assessment.RearLeftPhotoPath;
				RearRightPhoto.ImagePath = assessment.RearRightPhotoPath;
				CarDocument.ImagePath = assessment.CarDocumentPhotoPath;
				// ServiceRecordBook.ImagePath = assessment.ServiceRecordBookPhotoPath;
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

			fillSignature(Signature, AssignmentLetter);
			fillSignature(Signature1, AdvocateLetter);
			emptyNumericFields();
			Title = $"{assessment.OwnerName} - {assessment.LicensePlateClient} ({assessment.Id}/{assessment.ObjectId})";

		}

		private bool isZero(String text) {
			var result = 0f;
			float.TryParse(text, out result);
			return result == 0f;
		}

		private void emptyNumericFields() {
			foreach (var field in numericFieldsToUpdate) {
				if (isZero(field.Text)) {
					field.Text = "";
				}
			}
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
				mainScrollView.ScrollToAsync(0, 0, true);
				dialogOuterBox.IsVisible = false; //true;
			});
		});
		public override bool PrevArrowButtonVisiblity { get => PrevArrowButton.IsVisible; set => PrevArrowButton.IsVisible = value; }
		public override bool NextArrowButtonVisiblity { get => NextArrowButton.IsVisible; set => NextArrowButton.IsVisible = value; }

		async void AddNewPreDamageImage_Clicked(System.Object sender, System.EventArgs e) {
			await Shell.Current.GoToAsync(nameof(CameraPage));
		}

		protected override bool OnBackButtonPressed() {
			return DisplayAlert("Verlassen?",
	"Wollen Sie wirklich die Eingabe verlasen? All Ihre neu eingegebenen Daten werden nicht gespeichert?", "Ja", "Nein").Result;
		}

		async void CancelButton_Clicked(System.Object sender, System.EventArgs e) {
			if (await DisplayAlert("Verlassen?",
	"Wollen Sie wirklich die Eingabe verlasen? All Ihre neu eingegebenen Daten werden nicht gespeichert?", "Ja", "Nein")) {
				await Shell.Current.GoToAsync("..");
			}
		}

		internal override void HandleSpecialFields(int displayedGroup) {
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

		async Task<string> checkAndPersistSignature(SignaturePadView signature, string kind) {
			if (signature.IsBlank) {
				return null;
			}
			var signatureImgStream = await signature.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Jpeg);
			var mmm = new MemoryStream();
			using (MemoryStream ms = new MemoryStream()) {
				await (signatureImgStream as Stream).CopyToAsync(ms);
				var bytes = ms.ToArray();
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				var path = Path.Combine(documents, $"signature_{kind}_{Assessment.Id}.jpg");
				File.WriteAllBytes(path, bytes);
				return path;
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

		async Task sendPictures(IProgressDialog progress, ImagePathList imageList) {
			var httpRepository = HttpRepository.Instance;
			var assessmentId = Assessment.Id;
			int i = 3;
			
			foreach (var imagePath in imageList.ActiveImageList) {
				await httpRepository.PostPicture(imagePath,assessmentId);
				if (progress != null) {
					Device.BeginInvokeOnMainThread(() =>
						progress.PercentComplete = i * 100 / (imageList.ActiveImageList.Count + 2));
				}
				i++;
			}
		}

		async void SaveButton_Clicked(System.Object sender, System.EventArgs e) {
			String assignmentSignature = null;
			String advocateSignature = null;
			try {
				assignmentSignature = await checkAndPersistSignature(Signature, AssignmentLetter);
				advocateSignature = await checkAndPersistSignature(Signature1, AdvocateLetter);
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
					Assessment.LastSaved = DateTime.Now;
					await DataStore.UpdateItemAsync(Assessment);
				}


			} catch (Exception ex) {
				await DisplayAlert("Fehler", $"Fehler wärend des Speicherns.\n{ex.Message} in Zeile {ex.StackTrace[1]}", "OK");
			}
			if (sender == SaveSendButton) {
				var response = await HttpRepository.Instance.GetCredits();
				if (response == null || !response.StartsWith("CarAssessment")) {
					await DisplayAlert("Fehler", "Senden nicht gelungen, bitte ggf. später bei besserer Verbindung versuchen", "OK");
					return;
				}
				
				IProgressDialog progress = UserDialogs.Instance.Progress("Sende Daten", show: false);
				progress.Show();
				//var displayThread = new Thread(new ThreadStart(() =>
					//Device.BeginInvokeOnMainThread(() => progress.Show())));
				//displayThread.Start();

				var imageList = new ImagePathList(Assessment);
				if (assignmentSignature != null) {
					await HttpRepository.Instance.PostPicture(assignmentSignature);
					if (progress != null) {
						progress.PercentComplete = 100 / (imageList.ActiveImageList.Count+2);
					}
				}

				if (advocateSignature != null) {
					await HttpRepository.Instance.PostPicture(advocateSignature);
					if (progress != null) {
						progress.PercentComplete = 200 / (imageList.ActiveImageList.Count + 2);
					}
				}

				try {
					await sendPictures(progress, imageList);
					if (Assessment.ObjectId < 1) {
						await HttpRepository.Instance.PostAssessment(Assessment);
					} else {
						await HttpRepository.Instance.PutAssessment(Assessment);
					}
					await DataStore.UpdateItemAsync(Assessment);
				} catch (Exception ex) {
					await DisplayAlert("Fehler", $"Fehler wärend des Sendens.\n{ex.Message} in Zeile {ex.StackTrace[1]}", "OK");
					return;
				} finally {
					if (progress != null) {
						progress.Hide();
						progress.Dispose();
					}
					//displayThread.Abort();
				}
				
			}
			await DataStore.Commit();
			await Shell.Current.GoToAsync("..");
		}

		void SaveSendButton_Clicked(System.Object sender, System.EventArgs e) {
			SaveButton_Clicked(sender, e);
		}


		void Up(Object sender, EventArgs e) {
			LayoutController.Up();
			DialogOuterBox.IsVisible = false;
		}

		async void Previous(Object sender, EventArgs e) {
			LayoutController.Previous();
			await MainScrollView.ScrollToAsync(0, 0, true);
		}

		async void Next(Object sender, EventArgs e) {
			LayoutController.Next();
			await MainScrollView.ScrollToAsync(0, 0, true);
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

		internal override void EnterDeclarationOfAssignment() {
			Owner.Text = Assessment.OwnerName;
			PlateOwner.Text = Assessment.LicensePlateClient;
			PlateOpponent.Text = Assessment.LicensePlateOponent;
			Address.Text = Assessment.Street + "\n" + Assessment.City;
			AdmissionDate.Text = Assessment.AdmissionDate.ToString("dd.MM.yyyy");
			AccidentDate.Text = Assessment.AccidentDate.ToString("dd.MM.yyyy");
			CityAndDate.Text = Assessment.City + ", den " + Assessment.AdmissionDate.ToString("dd.MM.yy");
		}

		internal override void EnterAdvocateAssignment() {
			Accident.Text = "Schadenfall vom " + Assessment.AccidentDate.ToString("dd.MM.yyyy") + ", Kfz-Kennzeichen: " + Assessment.LicensePlateClient;
			Advocate.Text = "Rechtsanwalt: "+ Assessment.RecommendedAdvocate;
			Advocate1.Text = "dem Rechtsanwalt: " + Assessment.RecommendedAdvocate;
			Client.Text = "Hiermit erteile ich " + Assessment.OwnerName + " " + Assessment.Street + ", " + Assessment.City;
			CityAndDate1.Text = Assessment.City + ", den " + Assessment.AdmissionDate.ToString("dd.MM.yy");
		}

		private void copyTireText(TitledEntryField from, TitledEntryField to) {
			if (from.Tag == to.Text) {
				from.Tag = to.Text = from.Text;
			}
		}

		void FrontLeftManufacturer_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "Text") {
				copyTireText(FrontLeftManufacturer, FrontRightManufacturer);
			}
		}

		void FrontLeftSize_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "Text") {
				copyTireText(FrontLeftSize, FrontRightSize);
			}
		}

		void RearLeftSize_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "Text") {
				copyTireText(RearLeftSize, RearRightSize);
			}
		}

		void RearLeftManufacturer_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (e.PropertyName == "Text") {
				copyTireText(RearLeftManufacturer, RearRightManufacturer);
			}
		}
	}
}
