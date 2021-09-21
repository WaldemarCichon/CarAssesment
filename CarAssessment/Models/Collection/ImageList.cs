using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CarAssessment.Models.Collection {
	public class ImageList : List<Image> {

		public List<String> PathList { get; }

		public ImageList(List<String> pathList) {
			pathList.ForEach((path) => { var image = new Image(); image.Source = path; base.Add(image); });
			PathList = pathList;
		}

		public new void Add(Image image) {
			base.Add(image);
			PathList.Add(GetPath(image.Source));
		}

		public new void Remove(Image image) {
			base.Remove(image);
			PathList.Remove(GetPath(image.Source));
		}

		private string GetPath(ImageSource imageSource) {
			if (imageSource.GetType() == typeof(FileImageSource)) {
				return (imageSource as FileImageSource).File;
			}
			return (imageSource as UriImageSource).Uri.ToString();
		}
	}
}
