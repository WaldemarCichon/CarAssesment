using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CarAssessment.Services;
using CarAssessment.Views;

namespace CarAssessment {
	public partial class App : Application {

		public App() {
			InitializeComponent();

			DependencyService.Register<MockDataStore>();

			MainPage = new AppShell();
			Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
		}

		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
