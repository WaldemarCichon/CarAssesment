using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CarAssessment.Services;
using CarAssessment.Views;
using System.IO;
using LiteDB;

namespace CarAssessment {
	public partial class App : Application {
		private LiteDatabase Database;
		public App() {
			InitializeComponent();

			DependencyService.Register<LiteDatabaseDataStore>();

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
