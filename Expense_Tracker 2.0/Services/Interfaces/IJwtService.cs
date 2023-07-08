using System.Security.Claims;

namespace Expense_Tracker_2._0.Services.Interfaces
{
    public interface IJwtService
    {
        int GetUserIdFromToken(ClaimsPrincipal user);
    }
}
