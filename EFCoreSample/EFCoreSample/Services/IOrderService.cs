using System.Security.Claims;
using EFCoreSample.Models.Request;
using EFCoreSample.Models.Response;

namespace EFCoreSample.Services;

public interface IOrderService
{
    bool AddOrder(List<AddOrderRequest> addOrderRequests, Guid userId);
    IEnumerable<GetOrderDetailsResponse> GetOrderDetails(Guid userId);
}