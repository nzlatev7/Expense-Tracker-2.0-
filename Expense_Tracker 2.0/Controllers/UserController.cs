using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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

            user.Email = EmailValidation(request.Email) ? user.Email = request.Email : null;

            if (user.Email == null)
            {
                return BadRequest("Invalid Email");
            }    
            
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return Ok();
        }
        static bool EmailValidation(string email)
        {
            string emialPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

            Match match = Regex.Match(email, emialPattern);

            if (match.Success)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
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