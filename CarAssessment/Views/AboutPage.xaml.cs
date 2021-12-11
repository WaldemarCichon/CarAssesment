using System;
using System.ComponentModel;
using System.Threading;
using CarAssessment.Models.Row;
using CarAssessment.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssessment.Views {
	public partial class AboutPage : ContentPage {
		private readonly LiteDatabaseDataStore store = DependencyService.Get<IDataStore<Assessment>>() as LiteDatabaseDataStore; // TODO must be changed to IDataStore<User>

		public AboutPage() {
			InitializeComponent();
			if (DeviceInfo.Idiom == DeviceIdiom.Phone) {
				TitleLabel.FontSize = 30;
				CopyrightLabel.FontSize = 20;
				StreetLabel.FontSize = 20;
				CityLabel.FontSize = 20;
				
			}
		}

		async void LogoutButton_Clicked(System.Object sender, System.EventArgs e) {
			if (await DisplayAlert("Ausloggen", "Ausloggen und Anwendung beenden?", "Ja", "Nein")) {
				await store.DeleteUser();
				Thread.CurrentThread.Abort();
			}
		}
	}
}
