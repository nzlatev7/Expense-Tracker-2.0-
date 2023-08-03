using Expense_Tracker_2._0.Constants;
using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Expense_Tracker_2._0.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_2._0.Controllers
{
    [ApiController]
    [Authorize(Roles = "Customer, Admin")]
    [Route("[controller]/[action]")]
    public class ExpenseController : ControllerBase
    {
        private IJwtService _jwtService;
        private ExpenseTrackerDbContext _dbContext;

        public ExpenseController(
            ExpenseTrackerDbContext dbContext,
            IJwtService jwtService)
        {
            _dbContext = dbContext;
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
            if (request.Name.Length < AppConstants.NameMinLength 
                || request.Name.Length > AppConstants.NameMaxLength)
            {
                return BadRequest(ResponseMessages.NameLength);
            }
            
            if (request.Amount < AppConstants.AmountMin 
                || request.Amount > AppConstants.AmountMax)
            {
                return BadRequest(ResponseMessages.AmountInvalid);
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
            if (request.Name.Length < AppConstants.NameMinLength
                || request.Name.Length > AppConstants.NameMaxLength)
            {
                return BadRequest(ResponseMessages.NameLength);
            }

            if (request.Amount < AppConstants.AmountMin
                || request.Amount > AppConstants.AmountMax)
            {
                return BadRequest(ResponseMessages.AmountInvalid);
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
