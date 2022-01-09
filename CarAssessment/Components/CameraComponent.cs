using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using CarAssessment.DataHandling;
using CarAssessment.Views;
using SkiaSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarAssessment.Components {
	public class CameraComponent {
		private Quaternion orientation;

		Image DisplayedImage { get; set; }
		public DisplayInfo DisplayInfo { get; private set; }

		public CameraComponent() {
			OrientationSensor.Start(SensorSpeed.UI);
			OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
			//DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
		}

		private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e) {
			this.DisplayInfo = e.DisplayInfo;
		}

		private void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e) {
			this.orientation = e.Reading.Orientation;
		}

		public CameraComponent(Image displayedImage):this() {
			DisplayedImage = displayedImage;
		}

		int calcRotation() {
			int rotation = 90;
			var absX = Math.Abs(orientation.X);
			var absY = Math.Abs(orientation.Z);
			if (absY < absX) {
				if (orientation.Z > 0) {
					rotation = 0;
				} else {
					rotation = 180;
				}
			} else {
				if (orientation.X > 0) {
					rotation = 270;
				}
			}
			rotation += 90;
			if (rotation == 360) {
				rotation = 0;
			}
			return rotation;
		}

		async public Task GetPhoto() {
			var newFile = getNewFileName();
			var photo = await MediaPicker.PickPhotoAsync();
			if (photo == null) {
				OrientationSensor.ReadingChanged -= OrientationSensor_ReadingChanged;
				OrientationSensor.Stop();
				return;
			}
			using (var stream = await photo.OpenReadAsync()) {
				using (var newStream = File.OpenWrite(newFile)) {
					await stream.CopyToAsync(newStream);
				}
			}
			if (this.DisplayedImage != null) {
				this.DisplayedImage.Source = newFile;
			}
			Device.BeginInvokeOnMainThread(() =>
				EntityRepository.Instance.CurrentPhotoField.Source = newFile
			);
			OrientationSensor.ReadingChanged -= OrientationSensor_ReadingChanged;
			OrientationSensor.Stop();
		}

		async public Task CapturePhoto() {
			var photo = await MediaPicker.CapturePhotoAsync();
			if (photo == null) {
				OrientationSensor.ReadingChanged -= OrientationSensor_ReadingChanged;
				OrientationSensor.Stop();
				return;
			}
			var filename = await LoadPhotoAsync(photo);
			if (this.DisplayedImage != null) {
				this.DisplayedImage.Source = filename;
			}
			Device.BeginInvokeOnMainThread(() =>
				EntityRepository.Instance.CurrentPhotoField.Source = filename
			);
			OrientationSensor.ReadingChanged -= OrientationSensor_ReadingChanged;
			OrientationSensor.Stop();
		}

		async Task<String> LoadPhotoAsync(FileResult photo) {
			var newFile = getNewFileName();
			var rotation = calcRotation();
			/*var rotation1 = this.DisplayInfo.Rotation;
			MainThread.BeginInvokeOnMainThread(() => {
				rotation1 = DeviceDisplay.MainDisplayInfo.Rotation;
			});
			switch (rotation1) {
				case DisplayRotation.Rotation0: rotation = 270; break;
				case DisplayRotation.Rotation90: rotation = 0; break;
				case DisplayRotation.Rotation180: rotation = 90; break;
				case DisplayRotation.Rotation270: rotation = 180; break;
			}*/
			using (var stream = await photo.OpenReadAsync()) {
				var stream1 = stream;
				if (rotation != 0) {
					stream1 = rotate(stream, rotation);
				}
				using (var newStream = File.OpenWrite(newFile)) {
					await stream1.CopyToAsync(newStream);
				}
			}
			return newFile;
		}

		private Stream rotate(Stream stream, int rotation) {
			return stream; 
			var bitmap = SKBitmap.Decode(stream);
			var rotatedBitmap = rotation == 180 ? new SKBitmap(bitmap.Width, bitmap.Height) : new SKBitmap(bitmap.Height, bitmap.Width);
			using (var surface = new SKCanvas(rotatedBitmap)) {
				switch (rotation) {
					case 90: surface.Translate(rotatedBitmap.Width, 0); break;
					case 180: surface.Translate(rotatedBitmap.Width, rotatedBitmap.Height); break;
					case 270: surface.Translate(0, rotatedBitmap.Height); break;
				}
				surface.RotateDegrees(rotation);
				surface.DrawBitmap(bitmap, 0, 0);
			}
			return SKImage.FromBitmap(rotatedBitmap).Encode().AsStream();
		}

		private string getNewFileName() {
			var assessmentId = DeviceInfo.Idiom == DeviceIdiom.Phone ? NewItemPagePhone.CurrentAssessment.Id : NewItemPage.CurrentAssessment.Id;
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var files = Directory.GetFiles(documents, "Pic." + assessmentId + ".*.HEIC");
			var max = "";
			int id = 1;
			foreach (var file in files) {
				if (string.Compare(file, max) > 0) {
					max = file;
				}
			}
			if (max.Length > 1 && max.Contains(".")) {
				var parts = max.Split(".");

				if (parts.Length > 3) {
					var idPart = parts[parts.Length - 2];
					id = int.Parse(idPart) + 1;
				}
			}
			return Path.Combine(documents, "Pic." + assessmentId + "." + id.ToString("000000") + ".HEIC");
		}
	}
}
