using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CarAssessment.Components;

using CarAssessment.Models;
using CarAssessment.ViewModels;

namespace CarAssessment.Views {
	public partial class NewItemPage : ContentPage {
		public Item Item { get; set; }
		class Damage {
			String x;
			int y;
			String z;
		}

		public NewItemPage() {
			InitializeComponent();
			BindingContext = new NewItemViewModel();
			var images = new List<Image>();
			images.Add(new Image() { Source = "IMG_2193.HEIC" });
			images.Add(new Image() { Source = "IMG_2194.HEIC" });
			images.Add(new Image() { Source = "IMG_2195.HEIC" });
			images.Add(new Image() { Source = "IMG_2196.HEIC" });
			images.Add(new Image() { Source = "IMG_2197.HEIC" });
			FrontRight.SourceList = images;
			RearLeft.ItemsSource = images;
			RearRight.ItemsSource = images;
			DamagePhotos.ItemsSource = images;
			NearbyPhotos.ItemsSource = images;
			DetailPhotos.ItemsSource = images;
			OtherPhotos.ItemsSource = images;
			var l = new List<Damage>();
			l.Add(new Damage());
			l.Add(new Damage());
			DamageListView.ItemsSource = l;
			PreDamage.ItemsSource = l;
		}

		async void AddNewPreDamageImage_Clicked(System.Object sender, System.EventArgs e) {
			await Shell.Current.GoToAsync(nameof(CameraPage));
		}
	}
}
