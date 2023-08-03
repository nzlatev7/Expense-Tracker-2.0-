using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Expense_Tracker_2._0.Constants;

namespace Expense_Tracker_2._0.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private ExpenseTrackerDbContext _dbContext;

        public AdminController(ExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPut]
        public ActionResult UpdateUser(AdminUpdateRequest request)
        {
            var userForUpdate = _dbContext.Users.Find(request.Id);

            bool isNotUniqueUsername = _dbContext.Users
                .Any(x => x.UserName == request.UserName);

            if (isNotUniqueUsername)
            {
                return BadRequest(ResponseMessages.UserNotUnique);
            }

            if (request.UserName.Length < AppConstants.UsernameMinLength
               || request.UserName.Length > AppConstants.UsernameMaxLength)
            {
                return BadRequest(ResponseMessages.UserNameLength);
            }

            if (request.Password.Length < AppConstants.PasswordMinLength
                || request.Password.Length > AppConstants.PasswordMaxLength)
            {
                return BadRequest(ResponseMessages.PasswordLength);
            }

            userForUpdate.UserName = request.UserName;
            userForUpdate.Password = request.Password;
            userForUpdate.Role = request.Role;
            _dbContext.SaveChanges();
            return Ok();
        }
        
        [HttpGet]
        public ActionResult<List<AdminGetAllUsersResponse>> GetUsersByPage(int pageNumber)
        {
            //page size, sorting options, or search filters - more functionalities
            // need to have an object for this data - good practise

            if (pageNumber <= 0)
            {
                return BadRequest(ResponseMessages.PageNumInvalid);
            }

            var users = _dbContext.Users
                .Skip((pageNumber - 1) * 10)
                .Take(10)
                .Select(x => new AdminGetAllUsersResponse()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Password = x.Password,
                    Role = x.Role,
                    Email = x.Email,
                }).ToList();

            return Ok(users);

            // implement pagination using (skip, take)

            // another way is to use 'yield return' which is a good choice
            // when we retrieve a big amount of data, lazy loading and streaming effect
            // here is more practical to use (skip,take),
            // bacause we can have 'RANDOM ACCESS' - jump to a particular page
        }

        [HttpGet]
        public async Task<IActionResult> StreamUsers()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");

            foreach (var user in GetAllUsersIterator())
            {
                // Construct SSE event data
                var eventData = $"data: {JsonSerializer.Serialize(user)}\n\n";

                // Write the event data to the response body stream
                await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(eventData));
                await Response.Body.FlushAsync();

                // Delay or perform additional logic between sending events
                await Task.Delay(1000);
            }

            return Ok();
        }

        [HttpDelete]
        public ActionResult DeleteUser(AdminDeleteRequest request)
        {  
            if (AppConstants.AdminPassword != request.Password)
            {
                return Unauthorized(ResponseMessages.PasswordInvalid);
            }

            var userForDelete = _dbContext.Users.Find(request.Id);

            _dbContext.Users.Remove(userForDelete);
            _dbContext.SaveChanges();

            return Ok();
        }

        private IEnumerable<AdminGetAllUsersResponse> GetAllUsersIterator()
        {
            foreach (var user in _dbContext.Users)
            {
                var userForReturning = new AdminGetAllUsersResponse()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    Role = user.Role,
                    Email = user.Email,
                };

                yield return userForReturning;
            }

            //Using yield return in combination with batch retrieval allows you
            //to implement lazy loading or paging mechanisms, where the interface
            //or consumer can request and process users in smaller chunks as needed.
        }
    }
}
