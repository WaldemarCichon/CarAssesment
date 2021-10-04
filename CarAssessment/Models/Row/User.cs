using System;

namespace CarAssessment.Models.Row {
	public class User: AbstractRow {
		public User() {
		}
		public String UserName { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public String Password { get; set; }
	}
}
