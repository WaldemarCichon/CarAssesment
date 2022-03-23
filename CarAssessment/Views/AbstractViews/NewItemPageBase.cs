using System;
using Xamarin.Forms;

namespace CarAssessment.Views.AbstractViews {
	public abstract class NewItemPageBase: ContentPage {
		public NewItemPageBase() {
		}

		public abstract bool PrevArrowButtonVisiblity { get; set; }
		public abstract bool NextArrowButtonVisiblity { get; set; }

		internal abstract void HandleSpecialFields(int displayedGroup);

		internal abstract void EnterDeclarationOfAssignment();


		internal abstract void EnterAdvocateAssignment();
	}
}
