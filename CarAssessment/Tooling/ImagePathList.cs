﻿using System;
using System.Collections.Generic;
using CarAssessment.Models.Row;

namespace CarAssessment.Tooling {
	public class ImagePathList {
		private List<String> imagePathList; 
		public ImagePathList(Assessment assessment) {
			imagePathList = new List<string>();
			addGeneralImagePathes(assessment);
			addDamageImagePathes(assessment);
			addPreDamageImagePathes(assessment);
		}

		private void addGeneralImagePathes(Assessment assessment) {
			imagePathList.AddRange(new String[] {
				assessment.FrontLeftPhotoPath,
				assessment.FrontRightPhotoPath,
				assessment.RearLeftPhotoPath,
				assessment.RearRightPhotoPath,
				assessment.ServiceRecordBookPhotoPath,
				assessment.CarDocumentPhotoPath,
				assessment.PoliceReportPhotoPath,
				assessment.ChassisNumberPhotoPath
			});
		}

		private void addDamageImagePathes(Assessment assessment) {
			imagePathList.AddRange(assessment.DamagePhotos);
			imagePathList.AddRange(assessment.NearbyPhotos);
			imagePathList.AddRange(assessment.OtherPhotos);
			imagePathList.AddRange(assessment.DetailPhotos);
		}

		private void addPreDamageImagePathes(Assessment assessment) {
			foreach (var preDamage in assessment.PreDamages) {
				imagePathList.Add(preDamage.ImagePath);
			}
		}

		public List<String> ActiveImageList {
			get {
				var activeImagePathList = new List<String>();
				foreach (var imagePath in imagePathList) {
					if (imagePath != null && imagePath != "") {
						activeImagePathList.Add(imagePath);
					}
				}
				return activeImagePathList;
			}
		}
	}
}
