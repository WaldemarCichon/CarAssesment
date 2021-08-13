using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;
using CarAssessment.Models.Row;

namespace CarAssessment.Model.Collection {
	public class AbstractCollection<T> : List<T> where T : AbstractRow, new() {

		private int id;
		public int Id {
			get {
				if (id == 0 && Count > 0) {
					id = Count;
				}
				return id;
			}
			set {
				id = value;
			}
		}

		public AbstractCollection() {

		}


		public virtual T New {
			get {
				T element = new T();
				element.Id = ++Id;
				return element;
			}
		}

		public T AddNew() {
			var newRecord = New;
			Add(newRecord);
			return newRecord;
		}
		public new void Add(T element) {
			if (element.Id == 0) {
				element.Id = ++Id;
			} else {
				if (Id < element.Id) {
					Id = element.Id;
				}
			}
			base.Add(element);
		}

		public new void Remove(T element) {
			Remove(element.Id);
		}

		public virtual void Remove(int id) {
			var foundElement = Find((element) => element.Id == id);
			base.Remove(foundElement);
		}

		public new T this[int i] {
			get {
				if (base[i].Id == i) {
					return base[i];
				}
				var element = this.Find((e) => e.Id == i);
				return element;
			}
		}

		public virtual void Renumber() {
			int i = 0;
			foreach (var subPosition in this) {
				subPosition.Id = ++i;
			}
			Id = i;
		}

		public T ElementFromJson(string json) {
			return JsonConvert.DeserializeObject<T>(json);
		}

		private String getPath() {
			string localPath = ""; //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			return System.IO.Path.Combine(localPath + this.GetType().Name + ".json");
		}

		public void LoadJson(String path) {
			var content = ""; // System.IO.File.ReadAllText(path);
			var jsonObjects = content.Split('|');
			foreach (var jsonObject in jsonObjects) {
				var record = ElementFromJson(jsonObject);
				Add(record);
			}
		}

		public void LoadJson() {
			LoadJson(getPath());
		}

		public void SaveJson() {
			var sb = new StringBuilder();
			foreach (var element in this) {
				sb.Append(element.ToJSonString()).Append('|');
			}
			//File.WriteAllText(getPath(), sb.ToString());
		}
	}
}
