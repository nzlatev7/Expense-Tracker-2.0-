using Expense_Tracker_2._0.Models.DB;

namespace Expense_Tracker_2._0.Services.Interfaces
{
    public interface IValidationToken
    {
        ValidationToken Generate(int userId);

        void Clear(string tokenValue);

        ValidationToken Resend(int userId);
    }
}
