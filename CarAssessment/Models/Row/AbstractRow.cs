using System;
using System.Text.Json;

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
		public int ObjectId { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastSaved { get; set; }
		public DateTime Sent { get; set; }

		public bool IsNewRow { get => Created > LastSaved; }

		public virtual void Persist() {
			// Persisted = DateTime.Now;
		}

		protected virtual void InitRow() { }

		public string ToJSonString() {
			return JsonSerializer.Serialize(this);
		}

		public virtual string Text => "???";
		public virtual string Detail => "???";
	}
}