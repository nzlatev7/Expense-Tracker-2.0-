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

        [HttpGet]
        public ActionResult<List<AdminGetAllResponse>> GetAllStepByStep(int pageNumber)
        {
            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be a positive integer.");
            }

            var users = _dbContext.Users
                .Skip((pageNumber - 1) * 10)
                .Take(10)
                .Select(x => new AdminGetAllResponse()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Password = x.Password,
                    Email = x.Email,
                }).ToList();

            return Ok(users);

            // implement pagination using (skip, take)

            // another way is to use 'yield return' which is a good choice
            // when we retrieve a big amount of data, lazy loading and streaming effect
            // here is more practical to use (skip,take),
            // bacause we can have 'RANDOM ACCESS' - jump to a particular page
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
