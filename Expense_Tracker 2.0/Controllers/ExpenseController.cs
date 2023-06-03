using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Expense_Tracker_2._0.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_2._0.Controllers
{
    [Authorize(Roles = "Customer, Admin")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ExpenseController : ControllerBase
    {
        private IConfiguration _configuration;
        private IJwtService _jwtService;
        private ExpenseTrackerDbContext _dbContext;

        public ExpenseController(
            ExpenseTrackerDbContext dbContext, 
            IConfiguration configuration, 
            IJwtService jwtService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        [HttpGet]
        public List<ExpenseGetAllResponse> GetAllByUserId()
        {
            int userId = _jwtService.GetUserIdFromToken(User);
            return _dbContext.Expenses.Where(x => x.UserId == userId).Select(x => new ExpenseGetAllResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Date = x.Date,
                Amount = x.Amount,
            }).ToList();
        }

        [HttpPost]
        public ActionResult Create(ExpenseCreateRequest request)
        {
            //Validation for Name Lenght, (name -> 1 - 30)
            if (request.Name.Length < 1 || request.Name.Length > 30)
            {
                return BadRequest("Name Lenght");
            }

            //Validation for Name Lenght, (amount -> 0 - 10000)
            if (request.Amount < 0 || request.Amount > 10000)
            {
                return BadRequest("Invalid Amount");
            }

            Expense newExpense = new Expense();
            {
                newExpense.Name = request.Name;
                newExpense.Type = request.Type;
                newExpense.Date = request.Date;
                newExpense.Amount = request.Amount;
                newExpense.UserId = _jwtService.GetUserIdFromToken(User);
            }
            _dbContext.Expenses.Add(newExpense);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public ActionResult Update(ExpenseUpdateRequest request)
        {
            //Validation for Name Lenght, (name -> 1 - 30)
            if (request.Name.Length < 1 || request.Name.Length > 30)
            {
                return BadRequest("Name Lenght");
            }
            //Validation for Name Lenght, (amount -> 0 - 10000)
            if (request.Amount < 0 || request.Amount > 10000)
            {
                return BadRequest("Invalid Amount");
            }

            var expenseForUpdate = _dbContext.Expenses.Find(request.Id);

            expenseForUpdate.Name = request.Name;
            expenseForUpdate.Type = request.Type;
            expenseForUpdate.Date = request.Date;
            expenseForUpdate.Amount = request.Amount;

            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(ExpenseDeleteRequest request)
        {
            var expenseForDelete = _dbContext.Expenses.Find(request.Id);
            if (expenseForDelete == null)
            {
                return NotFound();
            }
            
            _dbContext.Expenses.Remove(expenseForDelete);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
