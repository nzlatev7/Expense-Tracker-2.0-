using Expense_Tracker_2._0.Models.DB;
using System.Security.Claims;

namespace Expense_Tracker_2._0.Services.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(User user);
        int GetUserIdFromToken(ClaimsPrincipal user);
    }
}
