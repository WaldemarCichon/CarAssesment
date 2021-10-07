using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using CarAssessment.Models;
using CarAssessment.Views;
using CarAssessment.Models.Row;
using System.Collections.Generic;

namespace CarAssessment.ViewModels {
	public class ItemsViewModel : BaseViewModel {
		private Assessment _selectedItem;

		public ObservableCollection<Assessment> Assessments { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Assessment> ItemTapped { get; }

		public ItemsViewModel() {
			Title = "Browse";
			Assessments = new ObservableCollection<Assessment>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

			ItemTapped = new Command<Assessment>(OnItemSelected);

			AddItemCommand = new Command(OnAddItem);
		}

		async Task ExecuteLoadItemsCommand() {
			IsBusy = true;

			try {
				Assessments.Clear();
				var items = await DataStore.GetItemsAsync(true);
				var assessments = new List<Assessment>();
				foreach (var item in items) {
					assessments.Add(item);
				}
				assessments.Sort((assessment1, assessment2) => assessment2.Id - assessment1.Id); 
				foreach(var assessment in assessments) {
					Assessments.Add(assessment);
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex);
			} finally {
				IsBusy = false;
			}
		}

		internal async void Refresh() {
			await ExecuteLoadItemsCommand();
		}

		public void OnAppearing() {
			IsBusy = true;
			SelectedItem = null;
		}

		public Assessment SelectedItem {
			get => _selectedItem;
			set {
				SetProperty(ref _selectedItem, value);
				OnItemSelected(value);
			}
		}

		private async void OnAddItem(object obj) {
			await Shell.Current.GoToAsync(nameof(NewItemPage));
		}

		async void OnItemSelected(Assessment item) {
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			await Shell.Current.Navigation.PushAsync(new NewItemPage(item));
		}
	}
}
