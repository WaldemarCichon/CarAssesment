using System;
using System.Collections.Generic;
using CarAssessment.Models.Row;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class DamagePage : ContentPage {
		private NewItemPagePhone caller;
		private DamageDescription damageDescription;
		private DamageDescription currentDamageDescription;

		public DamagePage(NewItemPagePhone caller) : this(null, caller) {
			
		}

		public DamagePage(DamageDescription damageDescription, NewItemPagePhone caller) {
			InitializeComponent();
			this.caller = caller;
			this.damageDescription = damageDescription;
			if (damageDescription == null) {
				this.currentDamageDescription = new DamageDescription();
			} else {
				this.currentDamageDescription = new DamageDescription(damageDescription);
			}
			this.BindingContext = currentDamageDescription;
			if (currentDamageDescription.Hours == 0) {
				this.HoursField.Text = "";
			}
		}

		void SaveDamage_Clicked(System.Object sender, System.EventArgs e) {
			if (damageDescription == null) {
				caller.AddDamageDescription(currentDamageDescription);
			} else {
				damageDescription.copyFrom(currentDamageDescription);
			}
			caller.RefreshDamageDescriptions();
			Shell.Current.Navigation.PopAsync();
		}

		void CancelDamage_Clicked(System.Object sender, System.EventArgs e) {
			Shell.Current.Navigation.PopAsync();
		}
	}
}
