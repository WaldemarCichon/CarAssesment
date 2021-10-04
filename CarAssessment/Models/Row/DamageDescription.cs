using System;
namespace CarAssessment.Models.Row {
	public class DamageDescription {
		public DamageDescription() {			
		}

		public string Description { get; set; }
		public bool Paint { get; set; }
		public bool Replace { get; set; }
		public bool Repair { get; set; }
		public decimal Hours { get; set; }
	}
}
