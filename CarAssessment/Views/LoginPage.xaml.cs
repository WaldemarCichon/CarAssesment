using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAssessment.Models.Row;
using CarAssessment.REST;
using CarAssessment.Services;
using CarAssessment.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssessment.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage {
		public LoginPage() {
			InitializeComponent();
			this.BindingContext = user = new User();// new LoginViewModel();
		}

		private User user;

		public AppShell Shell { get; internal set; }

		async void LoginButton_Clicked(System.Object sender, System.EventArgs e) {
			if (user.UserName == "" || user.Password == "") {
				await DisplayAlert("Bitte ergänzen", "Login name oder Passwort fehlen", "OK");
			}
			if (await HttpRepository.Instance.Login(user.UserName.ToLower(), user.Password)) {
				user = HttpRepository.Instance.User;
				Shell.LoginSuccessed();
			} else {
				await DisplayAlert("Login nicht erfolgreich", "Ein Login mit den angegebenem User-Namen und/oder dem Passwort konnte nicht erfolgreich durchgeführt werden", "OK");
			}
		}
	}
}
