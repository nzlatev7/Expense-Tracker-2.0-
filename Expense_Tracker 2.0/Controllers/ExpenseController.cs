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
                Name = x.Name,
                Type = x.Type,
                Date = x.Date,
                Amount = x.Amount,
            }).ToList();
        }

        [HttpPost]
        public ActionResult Create(ExpenseCreateRequest request)
        {
            //validate information
            if (request.Name == String.Empty || request.Type == String.Empty
                || request.Date == String.Empty || request.Amount == 0)
            {
                return BadRequest("Information required");
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
