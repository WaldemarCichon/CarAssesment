using System;
namespace CarAssessment.Models.Row {
	public class DamageDescription {

		public DamageDescription() {
		}

		public DamageDescription(DamageDescription damageDescription) {
			copyFrom(damageDescription);
		}

		public void copyFrom(DamageDescription damageDescription) {
			this.Description = damageDescription.Description;
			this.Paint = damageDescription.Paint;
			this.Repair = damageDescription.Repair;
			this.Replace = damageDescription.Replace;
			this.Hours = damageDescription.Hours;

		}

		public string Description { get; set; }
		public bool Paint { get; set; }
		public bool Replace { get; set; }
		public bool Repair { get; set; }
		public decimal Hours { get; set; }
	}
}
