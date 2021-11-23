using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarAssessment.Models.Parts {
	public class Tire {
		public Tire() {

		}

		private String manufacturer;
		private String size;
		private decimal treadDepth;

		public event PropertyChangedEventHandler PropertyChanged;

		public String Manufacturer {
			get => manufacturer;
			set { manufacturer = value; OnPropertyChanged();}
		}
		public String Size {
			get => size;
			set { size = value; OnPropertyChanged(); }
		}
		public decimal TreadDepth {
			get => treadDepth;
			set { treadDepth = value; OnPropertyChanged(); }
		}

		void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
