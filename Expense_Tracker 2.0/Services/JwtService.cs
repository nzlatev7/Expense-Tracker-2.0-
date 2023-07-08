using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Services.Interfaces;
using System.Security.Claims;

namespace Expense_Tracker_2._0.Services
{
    public class JwtService : IJwtService
    {
        public int GetUserIdFromToken(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
        }
    }
}
