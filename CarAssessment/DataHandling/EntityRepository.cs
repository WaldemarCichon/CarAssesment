using System;
using CarAssessment.Components;
using CarAssessment.Models.Row;
using Xamarin.Forms;

// Small repository
namespace CarAssessment.DataHandling {
	public class EntityRepository {

		private static EntityRepository instance; 

		private EntityRepository() {

		}

		public Assessment CurrentAssesment { get; set; }
		public PhotoField CurrentPhotoField { get; set; }

		public static EntityRepository Instance {
			get {
				if (instance == null) {
					instance = new EntityRepository();
				}
				return instance;
			}
		}
	}
}
