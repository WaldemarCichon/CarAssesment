using System;
using System.Collections.Generic;
using System.IO;
using CarAssessment.DataHandling;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class CameraPage : ContentPage {
		public CameraPage() {
			InitializeComponent();

		}



		void MakePhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			Camera.CaptureMode = Xamarin.CommunityToolkit.UI.Views.CameraCaptureMode.Photo;
			
			Camera.Shutter();
			Shell.Current.Navigation.PopAsync();
		}

		void Camera_OnAvailable(System.Object sender, System.Boolean e) {
		}

		void Camera_MediaCaptured(System.Object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e) {
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var filename = Path.Combine(documents, "img"+new Random().Next()+".HEIC");
			File.WriteAllBytes(filename, e.ImageData);
			EntityRepository.Instance.CurrentPhotoField.Source = filename;
		}
	}
}
