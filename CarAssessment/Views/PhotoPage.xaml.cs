using System;
using System.Collections.Generic;
using CarAssessment.DataHandling;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class PhotoPage : ContentPage {
		public PhotoPage() {
			InitializeComponent();
			// var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			DisplayedImage.Source = EntityRepository.Instance.CurrentPhotoField.ImageSource;

			EntityRepository.Instance.CurrentPhotoField.PropertyChanged += (sender, args) => {
				if (args.PropertyName == "ImagePath") {
					MainThread.BeginInvokeOnMainThread(() => {
						this.DisplayedImage.Source = EntityRepository.Instance.CurrentPhotoField.ImageSource;
					});
				}
			};
		}

		void DeletePhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			Shell.Current.Navigation.PopAsync();
		}

		void MakePhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			Shell.Current.GoToAsync(nameof(CameraPage));
		}

	}
}
