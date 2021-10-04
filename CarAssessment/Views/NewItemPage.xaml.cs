﻿using System;
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

// Page is used as well for the new item but also for the editing
namespace CarAssessment.Views {
	public partial class NewItemPage : ContentPage {
        private static LayoutController LayoutController { get; set; }

        public Item Item { get; set; }
		class Damage {
			String x;
			int y;
			String z;
		}

		public Command TitleClicked { get; } = new Command((param) => LayoutController.DisplayedGroup = int.Parse(param as string));

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

			DamageDescriptions.ItemsSource = assessment.DamageDescriptions;
			var l = new List<Damage>();
			l.Add(new Damage());
			l.Add(new Damage());
		}

		async void AddNewPreDamageImage_Clicked(System.Object sender, System.EventArgs e) {
			await Shell.Current.GoToAsync(nameof(CameraPage));
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
			if (displayedGroup == 7) {
				PoliceReport.IsVisible = Assessment.IsPoliceReport == true;
				RecommendedAdvocate.IsVisible = Assessment.WantAdvocate == true && Assessment.IsRecommendedAdvocate==true;
				CustomersAdvocate.IsVisible = Assessment.WantAdvocate == true && Assessment.IsRecommendedAdvocate == false;
			}
		}

		internal void HandleSpecialFields(object sender, CheckedChangedEventArgs args) {
			HandleSpecialFields(int.Parse(((View)sender).AutomationId));
		}

		async void SaveButton_Clicked(System.Object sender, System.EventArgs e) {
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
	}
}
