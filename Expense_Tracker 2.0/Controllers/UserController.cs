using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Expense_Tracker_2._0.Services;
using Expense_Tracker_2._0.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Expense_Tracker_2._0.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ExpenseTrackerDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IValidationToken _validationToken;

        public UserController(
            ExpenseTrackerDbContext dbContext, 
            IConfiguration configuration, 
            IJwtService jwtService,
            IEmailService emailService,
            IValidationToken validationToken)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _jwtService = jwtService;
            _emailService = emailService;
            _validationToken = validationToken;
        }

        [HttpPost]
        public ActionResult Register(UserRegisterRequest request)
        {
            bool isNotUniqueUsername = _dbContext.Users
                .Select(x => x.UserName)
                .Contains(request.UserName);

            if (isNotUniqueUsername)
            {
                return BadRequest("Username is not unique");
            }

            if (request.UserName.Length < 4 || request.UserName.Length > 25)
            {
                return BadRequest("Username Length");
            }

            if (request.Password.Length < 8 || request.Password.Length > 25)
            {
                return BadRequest("Password Length");
            }

            //email validation
            if (_dbContext.Users.Any(x => x.Email == request.Email))
            {
                return BadRequest("There is already a user registered with this email address.");
            }
            
            request.Email = EmailValidation(request.Email) ? request.Email = request.Email : null;

            if (request.Email == null)
            {
                return BadRequest("Invalid Email");
            }


            User user = new User();
            user.UserName = request.UserName;
            user.Password = request.Password;
            user.Email = request.Email;
            user.Role = Role.Customer;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var token = _validationToken.Generate(user.Id);

            _emailService.SendAsync(
                new EmailSendAsyncRequest() 
                { 
                    RecipientEmail = user.Email,
                    TokenValue = token.Value,
                    ExpirationDate = token.ExpirationDate,
                    Subject = "Verify your Email Address!",
                    Body = $"This is the token: {token.Value} which expire on {token.ExpirationDate}"
                });

            return Ok();
        }

        [HttpPost]
        public ActionResult VerifyEmailToken(UserVerifyEmailTokenRequest request)
        {
            var token = _dbContext.ValidationTokens
                .Where(t => t.Value == request.TokenValue)
                .FirstOrDefault();

            if (token == null)
            {
                return BadRequest("Invalid verification code. Please enter the correct code or request a new one.");
            }

            if (token.ExpirationDate < DateTime.UtcNow)
            {
                //clear the token
                _validationToken.Clear(request.TokenValue);

                return BadRequest("Verification code has expired. Please request a new one.");
            }

            var userForVerification = _dbContext.Users
                .Where(x => x.Id == token.UserId)
                .First();

            userForVerification.IsEmailVerified = true;
            _dbContext.SaveChanges();

            //clear the token
            _validationToken.Clear(request.TokenValue);

            return Ok("Verified! You have successfully verified your account.");
        }

        [HttpPost]
        public ActionResult ResendEmailToken(UserResendEmailTokenRequest request)
        {
            var user = _dbContext.Users
                .Where(x => x.Email == request.Email)
                .FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Invalid email!");
            }

            var token = _validationToken.Resend(user.Id);

            _emailService.SendAsync(
                new EmailSendAsyncRequest()
                {
                    RecipientEmail = request.Email,
                    TokenValue = token.Value,
                    ExpirationDate = token.ExpirationDate,
                    Subject = "Verify your Email Address!",
                    Body = $"This is the token: {token.Value} which expire on {token.ExpirationDate}"
                });

            return Ok();
        }

        [HttpPost]
        public ActionResult Login(UserLoginRequest request)
        {
            var user = _dbContext.Users
                .Where(x => x.UserName == request.UserName && x.Password == request.Password)
                .FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Wrong username or password.");
            }

            if (!user.IsEmailVerified)
            {
                return BadRequest("Please verify your email address");
            }

            //else return the JWT token
            string token = CreateJwtToken(user);

            return Ok(token);
        }

        [Authorize(Roles = "Customer, Admin")]
        [HttpPut]
        public ActionResult Update(UserUpdateRequest request)
        {
            int id = _jwtService.GetUserIdFromToken(User);

            if (_dbContext.Users.Any(x => x.UserName == request.UserName))
            {
                return BadRequest("Username is not unique");
            }

            if (request.UserName.Length < 4 || request.UserName.Length > 25)
            {
                return BadRequest("Username Length");
            }

            if (request.Password.Length < 8 || request.Password.Length > 25)
            {
                return BadRequest("Password Length");
            }

            if (_dbContext.Users.Any(x => x.Email == request.Email))
            {
                return BadRequest("There is already a user registered with this email address.");
            }

            request.Email = EmailValidation(request.Email) ? request.Email = request.Email : null;

            if (request.Email == null)
            {
                return BadRequest("Invalid Email");
            }

            var userForUpdate = _dbContext.Users.Find(id);
            userForUpdate.UserName = request.UserName;
            userForUpdate.Password = request.Password;
            userForUpdate.Email = request.Email;

            _dbContext.SaveChanges();

            return Ok();
        }

        [Authorize(Roles = "Customer, Admin")]
        [HttpGet]
        public UserGetInfoResponse GetInfo()
        {
            int id = _jwtService.GetUserIdFromToken(User);

            return _dbContext.Users
                .Where(x => x.Id == id)
                .Select(x => new UserGetInfoResponse()
            {
                UserName = x.UserName,
                Password = x.Password,
                Email = x.Email,
            }).Single();
        }

        [Authorize(Roles = "Customer, Admin")]
        [HttpDelete]
        public ActionResult Delete()
        {
            int id = _jwtService.GetUserIdFromToken(User);
            var userForDelete = _dbContext.Users.Find(id);

            _dbContext.Users.Remove(userForDelete);
            _dbContext.SaveChanges();

            return Ok();
        }
        private bool EmailValidation(string email)
        {
            string emialPattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

            Match match = Regex.Match(email, emialPattern);

            if (match.Success)
            {
                return true;
            }

            return false;
        }

        private string CreateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                  new Claim(ClaimTypes.Role, user.Role.ToString()),
              }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}