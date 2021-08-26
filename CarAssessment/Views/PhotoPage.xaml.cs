using System;
using System.Collections.Generic;
using CarAssessment.DataHandling;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class PhotoPage : ContentPage {
		public PhotoPage() {
			InitializeComponent();
			DisplayedImage.Source = EntityRepository.Instance.CurrentPhotoField.Source;

			EntityRepository.Instance.CurrentPhotoField.PropertyChanged += (sender, args) => {
				if (args.PropertyName == "Source") {
					MainThread.BeginInvokeOnMainThread(() => {
						this.DisplayedImage.Source = EntityRepository.Instance.CurrentPhotoField.Source;
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
