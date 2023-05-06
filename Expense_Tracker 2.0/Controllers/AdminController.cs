using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Expense_Tracker_2._0.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private IConfiguration _configuration;
        private ExpenseTrackerDbContext _dbContext;
        public AdminController(ExpenseTrackerDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPut]
        public ActionResult Update(AdminUpdateRequest request)
        {
            //update's username - unique - need to add validation

            var userForUpdate = _dbContext.Users.Find(request.Id);
            userForUpdate.UserName = request.UserName;
            userForUpdate.Password = request.Password;
            userForUpdate.Role = request.Role;
            userForUpdate.Email = request.Email;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public List<AdminGetAllResponse> GetAll()
        {
            return _dbContext.Users.Select(x => new AdminGetAllResponse()
            {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email,
            }).ToList();
        }


        [HttpDelete]
        public ActionResult Delete(AdminDeleteRequest request)
        {
            var userForDelete = _dbContext.Users.Find(request.Id);
            _dbContext.Users.Remove(userForDelete);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
