using System.ComponentModel;
using Xamarin.Forms;
using CarAssessment.ViewModels;

namespace CarAssessment.Views {
	public partial class ItemDetailPage : ContentPage {
		public ItemDetailPage() {
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}
