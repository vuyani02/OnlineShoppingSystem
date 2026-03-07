namespace OnlineShoppingSystem.Repositories
{
    /// <summary>
    /// Generic repository interface that defines the standard
    /// data operations all repositories must implement.
    /// </summary>
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void SaveToJson();
    }
}