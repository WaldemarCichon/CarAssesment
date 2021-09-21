using System;
using Newtonsoft.Json;

namespace CarAssessment.Models.Row {
	public abstract class AbstractRow {
		public AbstractRow() {
			Created = DateTime.Now;
			InitRow();
		}

		protected AbstractRow(int id) : this() {
			Id = id;
		}

		public int Id { get; set; }
		public DateTime Created { get; private set; }
		public DateTime Persisted { get; private set; }

		public virtual void Persist() {
			Persisted = DateTime.Now;
		}

		protected virtual void InitRow() { }

		public string ToJSonString() {
			return JsonConvert.SerializeObject(this);
		}

		public virtual string Text => "???";
		public virtual string Detail => "???";
	}
}