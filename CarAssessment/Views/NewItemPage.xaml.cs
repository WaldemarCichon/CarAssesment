using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CarAssessment.Models;
using CarAssessment.ViewModels;

namespace CarAssessment.Views {
	public partial class NewItemPage : ContentPage {
		public Item Item { get; set; }

		public NewItemPage() {
			InitializeComponent();
			BindingContext = new NewItemViewModel();
		}
	}
}
