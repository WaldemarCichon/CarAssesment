using System;
using System.Collections.Generic;
using CarAssessment.DataHandling;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class CameraPage : ContentPage {
		public CameraPage() {
			InitializeComponent();

		}



		void MakePhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			Camera.Shutter();
			Shell.Current.Navigation.PopAsync();
		}

		void Camera_OnAvailable(System.Object sender, System.Boolean e) {
		}

		void Camera_MediaCaptured(System.Object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e) {
			EntityRepository.Instance.CurrentPhotoField.Source = e.Image;
		}
	}
}
