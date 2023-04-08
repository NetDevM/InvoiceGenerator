using InvoiceGenerator.Models;

namespace InvoiceGenerator.Interfaces
{
    public interface IPaymentService
    {
        
        Task<Payment> GetPaymentById(int paymentid);
        Task<bool> AddPayment(Payment payment);
        Task<bool> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(int paymentid);
        Task<List<Payment>> Payments();
        
    }
}
