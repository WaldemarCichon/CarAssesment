using System;
using System.ComponentModel;
using System.Threading;
using CarAssessment.Models.Row;
using CarAssessment.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssessment.Views {
	public partial class StartPage : ContentPage {
		private readonly LiteDatabaseDataStore store = DependencyService.Get<IDataStore<Assessment>>() as LiteDatabaseDataStore; // TODO must be changed to IDataStore<User>

		public StartPage() {
			InitializeComponent();
			if (DeviceInfo.Idiom == DeviceIdiom.Phone) {
				TitleLabel.FontSize = 30;
				CopyrightLabel.FontSize = 20;
				StreetLabel.FontSize = 20;
				CityLabel.FontSize = 20;
				ButtonStack.Orientation = StackOrientation.Vertical;
			}
		}

		private AppShell switchToItems() {
			var shell = Shell.Current as AppShell;
			shell.StartTab.IsVisible = false;
			shell.AssessmentTab.IsVisible = true;
			shell.CurrentItem = shell.AssessmentTab;
			return shell;
		}

		void ShowListButton_Clicked(System.Object sender, System.EventArgs e) {
			switchToItems();
			ItemsPage.NewAssessmentMode = CreationMode.None;
		}

		void CreateNewButton_Clicked(System.Object sender, System.EventArgs e) {
			var shell = switchToItems();
			ItemsPage.NewAssessmentMode = CreationMode.Overview;
		}

		void CreateNewButton_direct_Clicked(System.Object sender, System.EventArgs e) {
			var shell = switchToItems();
			ItemsPage.NewAssessmentMode = CreationMode.Direct;
		}
	}
}
