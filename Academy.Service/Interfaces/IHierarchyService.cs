using Academy.Domain.Responses;

namespace Academy.Service.Interfaces;

public interface IHierarchyService<T>
{
    Task<BaseResponse<T>> Create(T model);
    Task<BaseResponse<T>> Update(int id,T model);
    Task<BaseResponse<T>> Delete(int id);
    Task<BaseResponse<List<T>>> GetAll();
    Task<BaseResponse<T>> GetById(int id);
}