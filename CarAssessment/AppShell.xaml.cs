using System;
using System.Collections.Generic;
using CarAssessment.REST;
using CarAssessment.ViewModels;
using CarAssessment.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarAssessment {
	public partial class AppShell : Xamarin.Forms.Shell {
		public AppShell() {
			InitializeComponent();
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), DeviceInfo.Idiom == DeviceIdiom.Phone ? typeof(NewItemPagePhone) : typeof(NewItemPage));
			Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
			Routing.RegisterRoute(nameof(PhotoPage), typeof(PhotoPage));
			Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
			Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
			Routing.RegisterRoute(nameof(DamagePage), typeof(DamagePage));
			Routing.RegisterRoute(nameof(PreDamagePage), typeof(PreDamagePage));
			if (HttpRepository.Instance.User != null) {
				LoginSuccessed();
			}

			
		}

		public void LoginSuccessed() {
			Device.BeginInvokeOnMainThread(() => {
				LoginTab.IsVisible = false;
				StartTab.IsVisible = true;
				TabBar.CurrentItem = StartTab;
			});
		}

		void LoginTab_ChildAdded(System.Object sender, Xamarin.Forms.ElementEventArgs e) {
			(e.Element as LoginPage).Shell = this;
		}


	}
}
