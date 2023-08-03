using Expense_Tracker_2._0.Models.Request;

namespace Expense_Tracker_2._0.Services.Interfaces
{
    public interface IEmailService
    {
        bool IsValid(string email);
        Task SendAsync(EmailSendAsyncRequest request);
    }
}
