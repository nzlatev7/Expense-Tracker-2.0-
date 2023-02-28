using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_2._0.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ExpenseController : ControllerBase
    {
        private IConfiguration _configuration;
        private ExpenseTrackerDbContext _dbContext;
        public ExpenseController(ExpenseTrackerDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet]
        public List<ExpenseGetAllResponse> GetAll()
        {
            return _dbContext.Expenses.Select(x => new ExpenseGetAllResponse()
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
            //Validation for Information required
            if (request.Name == String.Empty || request.Type == String.Empty
                || request.Date == String.Empty || request.Amount == 0)
            {
                return BadRequest("Information required");
            }
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
            }
            _dbContext.Expenses.Add(newExpense);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public ActionResult Update(ExpenseUpdateRequest request)
        {
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
            var userForDelete = _dbContext.Expenses.Find(request.Id);
            if (userForDelete == null)
            {
                return NotFound();
            }
            
            _dbContext.Expenses.Remove(userForDelete);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
