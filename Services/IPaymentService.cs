using ZATO.DTOs;

namespace ZATO.Services;

public interface IPaymentService
{
    Task<bool> IsPaymentConfirmedAsync(string paymentStatus);
    Task ProcessPaymentAsync(OrderDTO orderDto);
}