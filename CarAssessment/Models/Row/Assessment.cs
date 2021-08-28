using System;
using CarAssessment.Models.Parts;

namespace CarAssessment.Models.Row {
	public class Assessment: AbstractRow {
		public Assessment() {
		}

		public String OwnerName { get; set; }
		public String Street { get; set; }
		public String ZipCode { get; set; }
		public String City { get; set; }
		public String LicensePlateClient { get; set; }
		public String LicensePlateOponent { get; set; }
		public String ChassisNumber { get; set; }

		public DateTime AdmissionDate { get; set; }
		public DateTime AccidentDate { get; set; }
		public String InspectionLocation { get; set; }
		public String InspectionAttendees { get; set; }
		public String PreviousDamage { get; set; }
		public Boolean? PreviousDamageFixed { get; set; }
		public String PreviousDamageNotFixed { get; set; }
		public Tire FrontLeft { get; set; }
		public Tire FrontRight { get; set; }
		public Tire RearLeft { get; set; }
		public Tire RearRight { get; set; }
		public Boolean? RoadWorthy { get; set; }
		public Boolean? ReadyToDrive { get; set; }
		public Boolean Repair { get; set; }
		public String WhereToRepair { get; set; }
		public decimal HourlyRateBody { get; set; }
		public decimal HourlyRateElectric { get; set; }
		public decimal HourlyRatePainting { get; set; }
		public decimal HourlyRatePaintingSurcharge { get; set; }
		public decimal HourlyRatePaintPoint { get; set; }
		public decimal UPESurcharge { get; set; }
		public decimal Transport { get; set; }
		public decimal SmallPartsSurcharge { get; set; }


		public override void Persist() {
			throw new NotImplementedException();
		}
	}
}
