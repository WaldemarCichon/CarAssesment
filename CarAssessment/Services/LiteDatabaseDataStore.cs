using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CarAssessment.Models.Row;
using LiteDB;

namespace CarAssessment.Services {
	class IdRecord {
		public Int16 Id { get; set; }
		public string Name { get; set; }
		public UInt16 Value { get; set; }
	}

	public class LiteDatabaseDataStore : IDataStore<Assessment> {
		public LiteDatabaseDataStore() {
			var prefix = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(prefix, "CarAssessment.dbl");
			Database = new LiteDatabase(path);
			Assessments = Database.GetCollection<Assessment>();
			Users = Database.GetCollection<User>();
			Ids = Database.GetCollection<IdRecord>();
			if (Ids.Count() == 0) {
				Ids.Insert(new IdRecord {
					Id = 1,
					Name = "AssessmentId",
					Value = 100
				});
				Database.Commit();
			}

			// Assessments.DeleteAll();
		}

		private LiteDatabase Database { get; }
		private ILiteCollection<Assessment> Assessments { get; }
		private ILiteCollection<User> Users { get; }
		private ILiteCollection<IdRecord> Ids;

		public async Task<bool> StartTransaction() {
			Database.BeginTrans();
			return await Task.FromResult(true);
		}

		public async Task<bool> Commit() {
			Database.Commit();
			return await Task.FromResult(true);
		}

		public async Task<bool> Rollback() {
			Database.Rollback();
			return await Task.FromResult(true);
		}

		public async Task<bool> AddItemAsync(Assessment assessment) {
			Assessments.Insert(assessment);
			return await Task.FromResult(true);
		}

		public async Task<Assessment> CreateItemAsync() {
			var idRecord = Ids.FindById(1);

			if (idRecord.Value == 0) {
				idRecord.Value = 100;
			}
			var id = idRecord.Value;
			idRecord.Value++;
			var result = Ids.Update(idRecord);
			Database.Commit();
			var newAssessment = new Assessment(id);
			// await AddItemAsync(newAssessment);
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

		public async Task<User> GetUser() {
			var users = Users.FindAll();
			if (Users.Count() == 0) {
				return null;
			}
			var enumerator = users.GetEnumerator();
			enumerator.MoveNext();
			return await Task.FromResult(enumerator.Current);
		}

		public async Task<bool> SaveUser(User user) {
			Users.DeleteAll();
			Users.Insert(user);
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteUser() {
			Users.DeleteAll();
			return await Task.FromResult(true);
		}
	}
}