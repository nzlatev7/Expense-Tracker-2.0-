using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_2._0.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;
        private ExpenseTrackerDbContext _dbContext;
        public UserController(ExpenseTrackerDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult Register(UserRegisterRequest request)
        {
            //validations
            User user = new User();
            user.UserName = request.UserName;
            user.Password = request.Password;
            user.Email = request.Email;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public ActionResult Login(UserLoginRequest request)
        {
            var user = _dbContext.Users.Any(x => x.UserName == request.UserName && x.Password == request.Password);
            if (!user)
            {
                return BadRequest("Wrong username or password.");
            }
            //else return the JWT token
            return Ok();
        }

        [HttpPut]
        public ActionResult Update(UserUpdateRequest request)
        {
            //update's username - unique
            var userForUpdate = _dbContext.Users.Find(request.Id);
            userForUpdate.UserName = request.UserName;
            userForUpdate.Password = request.Password;
            userForUpdate.Email = request.Email;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public List<UserGetAllResponse> GetAll()
        {
            return _dbContext.Users.Select(x => new UserGetAllResponse()
                {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email,
                Expenses = _dbContext.Expenses.Where(x => x.UserId == x.Id).ToList()
            }).ToList();
        }

        [HttpDelete]
        public ActionResult Delete(UserDeleteRequest request)
        {
            var userForDelete = _dbContext.Users.Find(request.Id);
            _dbContext.Users.Remove(userForDelete);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}