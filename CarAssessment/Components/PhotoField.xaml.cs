using System;
using System.Collections.Generic;
using CarAssessment.DataHandling;
using CarAssessment.Views;
using Xamarin.Forms;

namespace CarAssessment.Components {
	public partial class PhotoField : Grid {
		public PhotoField() {
			InitializeComponent();
		}

		public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(PhotoField), null, BindingMode.TwoWay);
		public ImageSource Source {
			get {
				return (ImageSource)GetValue(SourceProperty);
			}

			set {
				SetValue(SourceProperty, value);
				if (value == null) {
					Image.IsVisible = false;
					MakePhotoButton.IsVisible = true;
				} else {
					Image.IsVisible = true;
					MakePhotoButton.IsVisible = false;
				}
			}
		}

		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(PhotoField), default(string), BindingMode.OneWay);
		public String Title {
			get {
				return (String)GetValue(TitleProperty);
			}
			set {
				SetValue(TitleProperty,value);
			}
		}

		protected override void OnPropertyChanged(string propertyName = null) {
			base.OnPropertyChanged(propertyName);

			if (propertyName == TitleProperty.PropertyName) {
				TitleLabel.Text = Title;
			}
			if (propertyName == SourceProperty.PropertyName) {
				Image.Source = Source;
				if (Source == null) {
					Image.IsVisible = false;
					MakePhotoButton.IsVisible = true;
				} else {
					Image.IsVisible = true;
					MakePhotoButton.IsVisible = false;
				}
			}

		}

		void ImageSource_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Source = Image.Source;
		}

		async void MakePhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			EntityRepository.Instance.CurrentPhotoField = this;
			await Shell.Current.GoToAsync(nameof(CameraPage));
		}

		async void Image_Clicked(System.Object sender, System.EventArgs e) {
			EntityRepository.Instance.CurrentPhotoField = this;
			await Shell.Current.GoToAsync(nameof(PhotoPage));
		}
	}
}
