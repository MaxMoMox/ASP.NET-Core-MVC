using Academy.Domain.Enums;

namespace Academy.Domain.Responses;

public class BaseResponse<T>
{
    public string? Description { get; set; }
    public StatusCode StatusCode { get; set; }
    public T? Data { get; set; }
}