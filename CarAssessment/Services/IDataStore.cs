using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarAssessment.Services {
	public interface IDataStore<T> {
		Task<T> CreateItemAsync();
		T CreateItem();
		Task<bool> AddItemAsync(T item);
		Task<bool> UpdateItemAsync(T item);
		Task<bool> DeleteItemAsync(int id);
		Task<T> GetItemAsync(int id);
		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
		Task<bool> PersistAsync();
		Task<int> SendItemsAsync();
		Task<bool> Commit();
		Task<bool> Rollback();
		Task<bool> StartTransaction();
	}
}
