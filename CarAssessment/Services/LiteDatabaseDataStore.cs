using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CarAssessment.Models.Row;
using LiteDB;

namespace CarAssessment.Services {
	public class LiteDatabaseDataStore : IDataStore<Assessment> {
		public LiteDatabaseDataStore() {
			var prefix = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(prefix, "CarAssessment.dbl");
			Database = new LiteDatabase(path);
			Assessments = Database.GetCollection<Assessment>();
			id = Assessments.Count() == 0 ? 1 : (Assessments.Max((assessment) => assessment.Id)+1);
		}

		private int id;
		private LiteDatabase Database { get; }
		private ILiteCollection<Assessment> Assessments { get; }

		public async Task<bool> AddItemAsync(Assessment assessment) {
			Assessments.Insert(assessment);
			return await Task.FromResult(true);
		}

		public async Task<Assessment> CreateItemAsync() {
			var newAssessment = new Assessment(id++);
			await AddItemAsync(newAssessment);
			return await Task.FromResult(newAssessment);
		}

		public async Task<bool> DeleteItemAsync(int id) {
			Assessments.Delete(id);
			return await Task.FromResult(true);
		}

		public async Task<IEnumerable<Assessment>> GetItemsAsync(bool forceRefresh = false) {
			return await Task.FromResult<IEnumerable<Assessment>> (Assessments.FindAll());
		}

		public async Task<bool> PersistAsync() {
			Database.Commit();
			return await Task.FromResult(true);
		}

		public Task<int> SendItemsAsync() {
			throw new System.NotImplementedException();
		}

		public async Task<bool> UpdateItemAsync(Assessment assessment) {
			Assessments.Update(assessment);
			return await Task.FromResult(true);
		}

		public async Task<Assessment> GetItemAsync(int id) {
			return await Task.FromResult(Assessments.FindById(id));
		}
	}
}