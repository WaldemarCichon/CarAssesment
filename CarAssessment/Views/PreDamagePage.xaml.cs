using System;
using System.Collections.Generic;
using CarAssessment.Models.Row;
using Xamarin.Forms;

namespace CarAssessment.Views {
	public partial class PreDamagePage : ContentPage {
		private NewItemPagePhone caller;
		private PreDamage preDamage;
		private PreDamage currentPreDamage;

		public PreDamagePage(NewItemPagePhone caller): this(null, caller) {
		}

		public PreDamagePage(PreDamage preDamage, NewItemPagePhone caller) {
			InitializeComponent();
			this.caller = caller;
			this.preDamage = preDamage;
			if (preDamage == null) {
				this.currentPreDamage = new PreDamage();
			} else {
				this.currentPreDamage = new PreDamage(preDamage);
			}
			this.BindingContext = currentPreDamage;
		}

		void SavePreDamage_Clicked(System.Object sender, System.EventArgs e) {
			if (preDamage == null) {
				caller.AddPreDamage(currentPreDamage);
			} else {
				preDamage.copyFrom(currentPreDamage);
			}
			caller.RefreshPreDamages();
			Shell.Current.Navigation.PopAsync();
		}

		void CancelPreDamage_Clicked(System.Object sender, System.EventArgs e) {
			Shell.Current.Navigation.PopAsync();
		}

	}
}
