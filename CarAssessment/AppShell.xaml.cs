using System;
using System.Collections.Generic;
using CarAssessment.ViewModels;
using CarAssessment.Views;
using Xamarin.Forms;

namespace CarAssessment {
	public partial class AppShell : Xamarin.Forms.Shell {
		public AppShell() {
			InitializeComponent();
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
		}

	}
}
