using System;
using System.Collections.Generic;
using CarAssessment.DataHandling;
using CarAssessment.Models.Collection;
using CarAssessment.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarAssessment.Components {
	public partial class PhotoCollectionField : Grid {
		public PhotoCollectionField() {
			InitializeComponent();
		}

		public static readonly BindableProperty SourceListProperty = BindableProperty.Create(nameof(SourceList), typeof(List<Image>), typeof(PhotoCollectionField), null, BindingMode.OneWay);
		public ImageList SourceList {
			get {
				return (ImageList)GetValue(SourceListProperty);
			}

			set {
				SetValue(SourceListProperty, value);

			}
		}

		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(PhotoCollectionField), default(string), BindingMode.OneWay);
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
			if (propertyName == SourceListProperty.PropertyName) {
				PhotoCollection.ItemsSource = SourceList;
			}

		}

		async void MakePhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			EntityRepository.Instance.CurrentPhotoField = new PhotoField();
			EntityRepository.Instance.CurrentPhotoField.PropertyChanged += (senderProp, args) => {
				if (args.PropertyName == "Source") {
					MainThread.BeginInvokeOnMainThread(() => {
						var image = new Image();
						image.Source = EntityRepository.Instance.CurrentPhotoField.Source;
						SourceList.Add(image);
						PhotoCollection.ItemsSource = null; PhotoCollection.ItemsSource = SourceList;
					});
				}
			};
			await new CameraComponent().CapturePhoto();
		}

		async void GetPhotoButton_Clicked(System.Object sender, System.EventArgs e) {
			EntityRepository.Instance.CurrentPhotoField = new PhotoField();
			EntityRepository.Instance.CurrentPhotoField.PropertyChanged += (senderProp, args) => {
				if (args.PropertyName == "Source") {
					MainThread.BeginInvokeOnMainThread(() => {
						var image = new Image();
						image.Source = EntityRepository.Instance.CurrentPhotoField.Source;
						SourceList.Add(image);
						PhotoCollection.ItemsSource = null; PhotoCollection.ItemsSource = SourceList;
					});
				}
			};
			await new CameraComponent().GetPhoto();
		}

		async void Image_Clicked(System.Object sender, System.EventArgs e) {
			EntityRepository.Instance.CurrentPhotoField = new PhotoField();
			EntityRepository.Instance.CurrentPhotoField.Source=((ImageButton)sender).Source;
			EntityRepository.Instance.CurrentPhotoField.PropertyChanged += (senderProp, args) => {
				if (args.PropertyName == "Source") {
					MainThread.BeginInvokeOnMainThread(() => {
						((ImageButton)sender).Source = EntityRepository.Instance.CurrentPhotoField.Source;
					});
				}
			};
			await Shell.Current.GoToAsync(nameof(PhotoPage));
		}


	}
}
