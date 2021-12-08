using System;
using System.Collections.Generic;
using CarAssessment.REST;
using CarAssessment.ViewModels;
using CarAssessment.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarAssessment {
	public partial class AppShell : Xamarin.Forms.Shell {
        public DevicePlatform PlatformKind { get; private set; }
        public TargetIdiom DeviceKind { get; private set; }

        public AppShell() {
			InitializeComponent();
			checkDeviceCapabilities();
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
			Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
			Routing.RegisterRoute(nameof(PhotoPage), typeof(PhotoPage));
			Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
			Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
			if (HttpRepository.Instance.User != null) {
				LoginSuccessed();
			}
		}

	    private void checkDeviceCapabilities()
        {
			PlatformKind = DeviceInfo.Platform;
			DeviceKind = Device.Idiom;
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
