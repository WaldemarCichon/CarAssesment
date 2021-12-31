using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CarAssessment.Models.Parts;
using LiteDB;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CarAssessment.Models.Row {
	public class Assessment : AbstractRow {
		private static readonly DateTime EmptyDateTime = new DateTime(); 

		public Assessment() {
			
		}

		public Assessment(int id) : base(id) {
		}

		protected override void InitRow() {
			base.InitRow();

			if (FrontLeft == null) {
				FrontLeft = new Tire();
			}

			if (FrontRight == null) {
				FrontRight = new Tire();
			}

			if (RearLeft == null) {
				RearLeft = new Tire();
			}

			if (RearRight == null) {
				RearRight = new Tire();
			}

			if (DamagePhotos == null) {
				DamagePhotos = new List<string>();
			}
			if (NearbyPhotos == null) {
				NearbyPhotos = new List<string>();
			}

			if (DetailPhotos == null) {
				DetailPhotos = new List<string>();
			}

			if (OtherPhotos == null) {
				OtherPhotos = new List<string>();
			}

			if (DamageDescriptions == null) {
				DamageDescriptions = new List<DamageDescription>();
			}

			if (PreDamages == null ) {
				PreDamages = new List<PreDamage>();
			}
		}

		private DateTime tryParseDate(string dateString)
        {
			var result = EmptyDateTime;
			DateTime.TryParse(dateString, out result);
			return result;
        }

		public int UserId { get; set; }
		public String OwnerName { get; set; }
		public String Address { get; set; }
		public String Street { get; set; }
		public String ZipCode { get; set; }
		public String City { get; set; }
		public String LicensePlateClient { get; set; }
		public String LicensePlateOponent { get; set; }
		public String ChassisNumber { get; set; }
		public decimal Mileage { get; set; }
		public DateTime AdmissionDate { get; set; }
		public String AdmissionDateS
        {
			get => AdmissionDate == EmptyDateTime ? "" : AdmissionDate.ToString("dd.MM.yyyy");
			set => AdmissionDate = value == "" || value == null ? EmptyDateTime : tryParseDate(value); 
        }
		public DateTime AccidentDate { get; set; }
		public String AccidentDateS
		{
			get => AccidentDate == EmptyDateTime ? "" : AccidentDate.ToString("dd.MM.yyyy");
			set => AccidentDate = value == "" || value == null ? EmptyDateTime : tryParseDate(value);
		}
		public String AccidentTimestampS {
			get => AccidentTimestamp == EmptyDateTime ? "" : AccidentTimestamp.ToString("dd.MM.yyyy hh:mm");
			set => AccidentTimestamp = value == "" || value == null ? EmptyDateTime : tryParseAndAddDate(value);
		}

		private DateTime tryParseAndAddDate(string stringVal) {
			var dateTime = tryParseDate(stringVal);
			if (dateTime == EmptyDateTime) {
				return dateTime;
			}
			if (dateTime.Year <= 2000 && AccidentDate != EmptyDateTime) {
				dateTime = new DateTime(AccidentDate.Year, AccidentDate.Month, AccidentDate.Day, dateTime.Hour, dateTime.Minute, 0);
			}
			return dateTime;
		}


		public String InspectionLocation { get; set; }
		public String InspectionAttendees { get; set; }
		public String PreviousDamage { get; set; }
		public Boolean PreviousDamageFixed { get; set; }
		public String PreviousDamageNotFixed { get; set; }
		public Tire FrontLeft { get; set; }
		public Tire FrontRight { get; set; }
		public Tire RearLeft { get; set; }
		public Tire RearRight { get; set; }
		public Boolean IsRoadWorthy { get; set; }
		public Boolean IsNotRoadWorthy { get; set; }
		public Boolean IsReadyToDrive { get; set; }
		public Boolean IsNotReadyToDrive { get; set; }
		public Boolean Repair { get; set; }
		public String WhereToRepair { get; set; }
		public decimal HourlyRateBody { get; set; }
		public decimal HourlyRateElectric { get; set; }
		public decimal HourlyRatePainting { get; set; }
		public decimal FlatRatePainting { get; set; }
		public decimal HourlyRatePaintingSurcharge { get; set; }
		public decimal HourlyRatePaintPoint { get; set; }
		public decimal UPESurcharge { get; set; }
		public decimal Transport { get; set; }
		public decimal TimedRateTransport { get; set; }
		public decimal FlatrateTransport { get; set; }
		public decimal SmallPartsSurcharge { get; set; }
		public Boolean RentalCar { get; set; }
		public Boolean IsEligible { get; set; }
		public String Description { get; set; }
		public Boolean MustSlowDown { get; set; }
		public Boolean WasDriving { get; set; }
		public Boolean DroveAgainst { get; set; }
		public Boolean CameFromMinorStreet { get; set; }
		public Boolean DroveBackwards { get; set; }
		public Boolean AgainstParkingCar { get; set; }
		public Boolean AgainstStoppedCustomer { get; set; }
		public Boolean AgainstDrivingCustomer { get; set; }
		public Boolean TooFast { get; set; }
		public Boolean OverRedLight { get; set; }
		public Boolean OverStop { get; set; }
		public Boolean ChangedLane { get; set; }
		public Boolean DisobeyingRightLeft { get; set; }
		public Boolean OverlookedCustomer { get; set; }
		public String AccidentLocation { get; set; }
		public DateTime AccidentTimestamp { get; set; }
		public String Miscellanious { get; set; }
		public String InsuranceId { get; set; }
		public String DamageId { get; set; }
		public String Email { get; set; }
		public String Phone { get; set; }
		public Boolean IsPoliceReport { get; set; }
		public Boolean WantAdvocate { get; set; }
		public Boolean IsRecommendedAdvocate { get; set; }
		public String RecommendedAdvocate { get; set; }
		public String CustomersAdvocate { get; set; }
		public Boolean IsServiceRecordBook { get; set; }

		public List<String> DamagePhotos { get; set; }
		public List<String> NearbyPhotos { get; set; }
		public List<String> DetailPhotos { get; set; }
		public List<String> OtherPhotos { get; set; }

		public String FrontRightPhotoPath { get; set; }
		public String FrontLeftPhotoPath { get; set; }
		public String RearRightPhotoPath { get; set; }
		public String RearLeftPhotoPath { get; set; }
		public String CheckheftPhotoPath { get; set; }
		public String PoliceReportPhotoPath { get; set; }
		public String ServiceRecordBookPhotoPath { get; set; }
		public String CarDocumentPhotoPath { get; set; }
		public String SpeedometerPhotoPath { get; set; }
		public String ChassisNumberPhotoPath { get; set; }

		public List<DamageDescription> DamageDescriptions { get; set; }
		public List<PreDamage> PreDamages { get; set; }

		public String Line1 => OwnerName + " " + LicensePlateClient + " gegen " + LicensePlateOponent;
		public String Line2 => "Unfalldatum: " + AccidentDateS + ", Aufnahmnedatum: " + AdmissionDateS;

		private void addIfNotEmpty(List<string> list, string entry) {
			if (entry != null && entry.Length>0) {
				list.Add(entry);
			}
		}

		private void addIfNotEmpty(List<string> list, List<string> entries) {
			if (entries.Count > 0) {
				list.AddRange(entries);
			}
		}

		[JsonIgnore]
		[BsonIgnore]
		public List<string> PictureList {
			get {
				var list = new List<string>();
				addIfNotEmpty(list, FrontRightPhotoPath);
				addIfNotEmpty(list, FrontLeftPhotoPath);
				addIfNotEmpty(list, RearLeftPhotoPath);
				addIfNotEmpty(list, RearRightPhotoPath);
				addIfNotEmpty(list, ServiceRecordBookPhotoPath);
				addIfNotEmpty(list, PoliceReportPhotoPath);
				addIfNotEmpty(list, CarDocumentPhotoPath);
				return list;
			}
		}


		[JsonIgnore]
		[BsonIgnore]
		public Color BackgroundColor { get; set; } = Color.Beige;

		public override void Persist() {
			throw new NotImplementedException();
		}
	}


}
