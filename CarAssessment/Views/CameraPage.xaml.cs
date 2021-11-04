using System;
using System.Collections.Generic;
using System.IO;
using CarAssessment.DataHandling;
using CarAssessment.Models.Row;
using CarAssessment.REST;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class CameraPage : ContentPage {
		static bool ImmediatelyCloseAfterShutterClick { get; set; } = false;

		public CameraPage() {
			InitializeComponent();

		}



		async void MakePhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			Camera.CaptureMode = Xamarin.CommunityToolkit.UI.Views.CameraCaptureMode.Photo;
			
			Camera.Shutter();
			if (ImmediatelyCloseAfterShutterClick) {
				await Shell.Current.Navigation.PopAsync();
			}
		}

		void Camera_OnAvailable(System.Object sender, System.Boolean e) {
		}

		private string getNewFileName() {
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var files = Directory.GetFiles(documents, "Image." + NewItemPage.CurrentAssessment.Id + ".*.HEIC");
			var max = "";
			foreach (var file in files) {
				if (string.Compare(file, max) > 0) {
					max = file;
				}
			}
			var parts = max.Split(".");
			int id = 1;
			if (parts.Length > 3) {
				var idPart = parts[parts.Length - 2];
				id = int.Parse(idPart) + 1;
			};
			return Path.Combine(documents, "Pic." + NewItemPage.CurrentAssessment.Id + "."+id.ToString("000000")+".HEIC");
		}

		async void Camera_MediaCaptured(System.Object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e) {
			var filename = getNewFileName();
			File.WriteAllBytes(filename, e.ImageData);
			
			Device.BeginInvokeOnMainThread(() => 
				EntityRepository.Instance.CurrentPhotoField.Source = filename
			);
			if (!ImmediatelyCloseAfterShutterClick) {
				await Shell.Current.Navigation.PopAsync();
			}
			_ = HttpRepository.Instance.PostPicture(filename);
		}
	}
}
