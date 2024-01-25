namespace ZATO.Contracts;

public interface IGenericRepository<T> where T : class
{
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByIdAsync(int Id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
}