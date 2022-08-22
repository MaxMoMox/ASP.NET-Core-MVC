namespace Academy.DAL.Interfaces;
public interface IBaseRepository<T>
{
    Task Create (T model);
    Task<T> Update (T model);
    Task Delete (T model);
    Task<List<T>> GetAll();
    Task<T?> GetById (int modelId);
}