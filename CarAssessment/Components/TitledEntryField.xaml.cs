using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CarAssessment.Components {
	public partial class TitledEntryField : StackLayout {

		public double FontSizeTitle { get; set; }
		public double FontSizeText { get; set; }
		public double FontSizeRatio { get; set; } = 1.0;

		public TitledEntryField() {
			InitializeComponent();
		}

		public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(TitledEntryField), default(string), BindingMode.TwoWay);
		public string Text {
			get {
				return (string)GetValue(TextProperty);
			}

			set {
				SetValue(TextProperty, value);
			}
		}


		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TitledEntryField), default(string), Xamarin.Forms.BindingMode.OneWay);
		public string Title {
			get {
				return (string)GetValue(TitleProperty);
			}

			set {
				SetValue(TitleProperty, value);
			}
		}


		protected override void OnPropertyChanged(string propertyName = null) {
			base.OnPropertyChanged(propertyName);

			if (propertyName == TitleProperty.PropertyName) {
				TitleLabel.Text = Title;
			}
			if (propertyName == TextProperty.PropertyName) {
				TextEntry.Text = Text;
			}

		}

		void TextEntry_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Text = TextEntry.Text;
		}
	}
}
