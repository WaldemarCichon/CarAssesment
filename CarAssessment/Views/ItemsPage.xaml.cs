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

namespace CarAssessment.Views {
	public partial class ItemsPage : ContentPage {
		ItemsViewModel _viewModel;
		IDataStore<Assessment> DataStore = DependencyService.Get<IDataStore<Assessment>>();


		public ItemsPage() {
			InitializeComponent();

			BindingContext = _viewModel = new ItemsViewModel();
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

		async void SendAssessementButton_Clicked(System.Object sender, System.EventArgs e) {
			var assessment = (sender as Button).CommandParameter as Assessment;
			if (assessment.ObjectId < 1) {
				await HttpRepository.Instance.PostAssessment(assessment);
			} else {
				await HttpRepository.Instance.PutAssessment(assessment);
			}
			await DataStore.UpdateItemAsync(assessment);
			await DataStore.Commit();
			_viewModel.Refresh();
		}
	}
}
