using System;
using System.Collections.Generic;
using System.IO;
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
				SetValue(ImagePathProperty, ImagePath);
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

		public static readonly BindableProperty ImagePathProperty = BindableProperty.Create(nameof(ImagePath), typeof(string), typeof(PhotoField), default(string), BindingMode.TwoWay);
		public String ImagePath {
			get {
				if (Image.Source == null) {
					return null;
				}
				if (Image.Source.GetType() == typeof(FileImageSource)) {
					return (Image.Source as FileImageSource).File;
				} else {
					return (Image.Source as UriImageSource).Uri.AbsolutePath;
				}
			}

			set {
				SetValue(ImagePathProperty, value);
				if (value != ImagePath) {
					SetValue(SourceProperty, value);
					Image.Source = value;
				}
			}
		}

		protected override void OnPropertyChanged(string propertyName = null) {
			base.OnPropertyChanged(propertyName);

			if (propertyName == TitleProperty.PropertyName) {
				TitleLabel.Text = Title;
			}

			if (propertyName == ImagePathProperty.PropertyName) {
				if (ImagePath != null) {
					var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					Image.Source = Path.Combine(documents, Path.GetFileName(ImagePath));
				} else {
					Image.Source = null;
				}
				if (ImagePath == null) {
					Image.IsVisible = false;
					MakePhotoButton.IsVisible = true;
				} else {
					Image.IsVisible = true;
					MakePhotoButton.IsVisible = false;
				}
			}

			if (propertyName == SourceProperty.PropertyName) {
				var source = Source;
				if (Source != null) {
					if (Source.GetType() == typeof(FileImageSource)) {
						var path = (Source as FileImageSource).File;
						if (path != null) {
							var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
							source = new FileImageSource();
							(source as FileImageSource).File = Path.Combine(documents, Path.GetFileName(path));
						}
					}
				}
				Image.Source = source;
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
