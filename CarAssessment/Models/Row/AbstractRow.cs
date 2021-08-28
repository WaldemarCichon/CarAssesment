using System;
using Newtonsoft.Json;

namespace CarAssessment.Models.Row {
	public abstract class AbstractRow {
		public AbstractRow() {
			InitRow();
		}

		protected AbstractRow(int id) : this() {
			Id = id;
		}

		public int Id { get; set; }

		public abstract void Persist();

		protected virtual void InitRow() { }

		public string ToJSonString() {
			return JsonConvert.SerializeObject(this);
		}

		public virtual string Text => "???";
		public virtual string Detail => "???";
	}
}