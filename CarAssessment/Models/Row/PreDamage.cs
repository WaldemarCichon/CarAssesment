using System;
namespace CarAssessment.Models.Row {
	public class PreDamage {

		public PreDamage() {

		}

		public PreDamage(PreDamage preDamage) {
			copyFrom(preDamage);			
		}

		public void copyFrom(PreDamage preDamage) {
			this.Description = preDamage.Description;
			this.ImagePath = preDamage.ImagePath;
			this.IsOldDamage = preDamage.IsOldDamage;
			this.TempImagePath = preDamage.TempImagePath;
			this.IsRepaired = preDamage.IsRepaired;
		}

		public string Description { get; set; }
		public string ImagePath { get; set; }
		public string TempImagePath { get; set; }
		public bool IsRepaired { get; set; }
		public bool IsOldDamage { get; set; }
	}
}
