using Expense_Tracker_2._0.Models.Request;

namespace Expense_Tracker_2._0.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailSendAsyncRequest request);
    }
}
