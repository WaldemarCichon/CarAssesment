using System;
using CarAssessment.Models.Enums;

namespace CarAssessment.Models.Parts {
	public class Damage {
		public Damage() {
		}

		public String Description { get; set; }
		public LaborKind LaborKind { get; set; }
		public decimal hours { get; set; }
	}
}
