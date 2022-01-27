using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CarAssessment.Models;
using CarAssessment.Views;
using CarAssessment.ViewModels;
using CarAssessment.REST;
using CarAssessment.Models.Row;
using CarAssessment.Services;
using CarAssessment.Tooling;
using System.IO;
using Acr.UserDialogs;
using System.Threading;

namespace CarAssessment.Views {
	public enum CreationMode {
		None,
		Overview,
		Direct
	}

	public partial class ItemsPage : ContentPage {
		ItemsViewModel _viewModel;
		IDataStore<Assessment> DataStore = DependencyService.Get<IDataStore<Assessment>>();

		public static CreationMode NewAssessmentMode { get; internal set; }

		public ItemsPage() {
			InitializeComponent();

			BindingContext = _viewModel = new ItemsViewModel();
			if (NewAssessmentMode != CreationMode.None) {
				_viewModel.OnAddItem(NewAssessmentMode);
			}
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			_viewModel.OnAppearing();
		}

		async void DeleteAssessmentButton_Clicked(System.Object sender, System.EventArgs e) {
			if (!await DisplayAlert("Gutachten löschen", "Soll das Gutachten tatsächlich gelöscht werden?", "Ja", "Nein")) {
				return;
			}
			var assessmentId = int.Parse((sender as Button).AutomationId);
			await DataStore.DeleteItemAsync(assessmentId);
			//BindingContext = null;
			_viewModel.Refresh();
			//BindingContext = _viewModel;
		}

		internal void NewAssessment(bool directMode) {
			_viewModel.OnAddItem(directMode);
		}

		async Task sendPictures(Assessment assessment, IProgressDialog progress, ImagePathList imageList) {
			var httpRepository = HttpRepository.Instance;
			var assessmentId = assessment.Id;
			var i = 3;
			foreach (var imagePath in imageList.ActiveImageList) {
				await httpRepository.PostPicture(imagePath, assessmentId);
				progress.PercentComplete = i*100/(imageList.ActiveImageList.Count+2);
			}
		}

		async Task sendSignature (Assessment assessment, string kind) {
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(documents, $"signature_{kind}_{assessment.Id}.jpg");
			if (File.Exists(path)) {
				await HttpRepository.Instance.PostPicture(path);
			}
		}

		async void SendAssessementButton_Clicked(System.Object sender, System.EventArgs e) {
			IProgressDialog progress = UserDialogs.Instance.Progress("Sende Daten");
			try {

				var response = await HttpRepository.Instance.GetCredits();
				if (response == null || !response.StartsWith("CarAssessment")) {
					await DisplayAlert("Fehler", "Senden nicht gelungen, bitte ggf. später versuchen", "OK");
					return;
				}
				var assessment = (sender as Button).CommandParameter as Assessment;

				var imageList = new ImagePathList(assessment);

				await sendSignature(assessment, NewItemPage.AssignmentLetter);
				if (progress != null) {
					progress.PercentComplete = 200 / (imageList.ActiveImageList.Count + 2);
				}

				await sendSignature(assessment, NewItemPage.AdvocateLetter);
				if (progress != null) {
					progress.PercentComplete = 200 / (imageList.ActiveImageList.Count + 2);
				}

				await sendPictures(assessment, progress, imageList);

				if (assessment.ObjectId < 1) {
					await HttpRepository.Instance.PostAssessment(assessment);
				} else {
					await HttpRepository.Instance.PutAssessment(assessment);
				}
				await DataStore.UpdateItemAsync(assessment);
				await DataStore.Commit();
				_viewModel.Refresh();
			} catch (Exception ex) {
				await DisplayAlert("Fehler", $"Fehler wärend des Sendens.\n{ex.Message} in Zeile {ex.StackTrace[1]}","OK");
			} finally {
				if (progress != null) {
					progress.Hide();
					progress.Dispose();
				}
			}

		}
	}
}
