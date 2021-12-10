using System;
using System.Collections.Generic;
using CarAssessment.Models.Row;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class DamagePage : ContentPage {
		private DamageDescription damageDescription;

		public DamagePage() {
			InitializeComponent();
		}

		public DamagePage(DamageDescription damageDescription): this() {
			this.damageDescription = damageDescription;
		}
	}
}
