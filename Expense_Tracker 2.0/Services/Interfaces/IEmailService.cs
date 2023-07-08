namespace Expense_Tracker_2._0.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string recipientEmail, int userId);
    }
}
