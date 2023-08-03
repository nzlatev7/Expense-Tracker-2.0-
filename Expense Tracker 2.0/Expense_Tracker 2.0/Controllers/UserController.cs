using Expense_Tracker_2._0.Constants;
using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Models.Request;
using Expense_Tracker_2._0.Models.Response;
using Expense_Tracker_2._0.Services;
using Expense_Tracker_2._0.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_2._0.Controllers
{
    [ApiController]
    [Authorize(Roles = "Customer, Admin")]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ExpenseTrackerDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IValidationToken _validationToken;
        private readonly ICloudService _cloudService;

        private readonly ILogger<UserController> _logger;

        public UserController(
            ExpenseTrackerDbContext dbContext, 
            IJwtService jwtService,
            IEmailService emailService,
            IValidationToken validationToken,
            ICloudService cloudService,
            ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _emailService = emailService;
            _validationToken = validationToken;
            _cloudService = cloudService;

            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(UserRegisterRequest request)
        {
            bool isNotUniqueUsername = _dbContext.Users
                .Select(x => x.UserName)
                .Contains(request.UserName);

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

            if (_dbContext.Users.Any(x => x.Email == request.Email))
            {
                return BadRequest(ResponseMessages.EmailExists);
            }
            
            request.Email = _emailService.IsValid(request.Email) ? request.Email = request.Email : null;

            if (request.Email == null)
            {
                return BadRequest(ResponseMessages.EmailInvalid);
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
                    Subject = AppConstants.EmailSubjectVerification,
                    Body = string.Format(AppConstants.EmailBodyVerification, token.Value, token.ExpirationDate)
                });

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyEmailToken(UserVerifyEmailTokenRequest request)
        {
            var token = _dbContext.ValidationTokens
                .Where(t => t.Value == request.TokenValue)
                .FirstOrDefault();

            if (token == null)
            {
                return BadRequest(ResponseMessages.InvalidVerificationCode);
            }

            if (token.ExpirationDate < DateTime.UtcNow)
            {
                _validationToken.Clear(request.TokenValue);

                return BadRequest(ResponseMessages.VerificationCodeExpired);
            }

            var userForVerification = _dbContext.Users
                .Where(x => x.Id == token.UserId)
                .First();

            userForVerification.IsEmailVerified = true;
            _dbContext.SaveChanges();

            _validationToken.Clear(request.TokenValue);

            return Ok(ResponseMessages.EmailVerified);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResendEmailToken(UserResendEmailTokenRequest request)
        {
            var user = _dbContext.Users
                .Where(x => x.Email == request.Email)
                .FirstOrDefault();

            if (user == null)
            {
                return BadRequest(ResponseMessages.EmailInvalid);
            }

            var token = _validationToken.Resend(user.Id);

            _emailService.SendAsync(
                new EmailSendAsyncRequest()
                {
                    RecipientEmail = request.Email,
                    TokenValue = token.Value,
                    ExpirationDate = token.ExpirationDate,
                    Subject = AppConstants.EmailSubjectVerification,
                    Body = string.Format(AppConstants.EmailBodyVerification, token.Value, token.ExpirationDate)
                }); 

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserLoginRequest request)
        {
            var user = _dbContext.Users
                .Where(x => x.UserName == request.UserName && x.Password == request.Password)
                .FirstOrDefault();

            if (user == null)
            {
                return BadRequest(ResponseMessages.WrongUsernameOrPass);
            }

            if (!user.IsEmailVerified)
            {
                return BadRequest(ResponseMessages.EmailVerificationRequired);
            }

            string token = _jwtService.CreateToken(user);

            // Some Exercise
            ////Log an informational message to the debug and console (by default)
            _logger.LogInformation($"The user: {user.UserName} is successfully log in!", user.UserName);

            return Ok(token);
        }

        
        [HttpPut]
        public ActionResult Update(UserUpdateRequest request)
        {
            int id = _jwtService.GetUserIdFromToken(User);

            if (_dbContext.Users.Any(x => x.UserName == request.UserName))
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

            if (_dbContext.Users.Any(x => x.Email == request.Email))
            {
                return BadRequest(ResponseMessages.EmailExists);
            }

            request.Email = _emailService.IsValid(request.Email) ? request.Email = request.Email : null;

            if (request.Email == null)
            {
                return BadRequest(ResponseMessages.EmailInvalid);
            }

            var userForUpdate = _dbContext.Users.Find(id);
            userForUpdate.UserName = request.UserName;
            userForUpdate.Password = request.Password;
            userForUpdate.Email = request.Email;

            _dbContext.SaveChanges();

            return Ok();
        }

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

        [HttpDelete]
        public ActionResult Delete()
        {
            int id = _jwtService.GetUserIdFromToken(User);
            var userForDelete = _dbContext.Users.Find(id);

            _dbContext.Users.Remove(userForDelete);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public ActionResult ResetPassword(UserResetPasswordRequest request)
        {
            var user = _dbContext.Users
                .FirstOrDefault(x => x.Email == request.Email);

            if (user == null)
            {
                return BadRequest(ResponseMessages.EmailInvalid);
            }


            ValidationToken token;

            if (_dbContext.ValidationTokens.Any(x => x.UserId == user.Id))
            {
                token = _validationToken.Resend(user.Id);
            }
            else
            {
                token = _validationToken.Generate(user.Id);
            }

            _emailService.SendAsync(new EmailSendAsyncRequest()
            {
                RecipientEmail = user.Email,
                TokenValue = token.Value,
                ExpirationDate = token.ExpirationDate,
                Subject = AppConstants.PasswordSubjectVerification,
                Body = string.Format(AppConstants.EmailBodyVerification, token.Value, token.ExpirationDate)
            });

            return Ok();
        }

        [HttpPost]
        public ActionResult VerifyResetPasswordToken(UserVerifyResetPasswordToken request)
        {
            var token = _dbContext.ValidationTokens
                .FirstOrDefault(x => x.Value == request.TokenValue);

            if (token == null)
            {
                return BadRequest(ResponseMessages.InvalidVerificationCode);
            }

            if (token.ExpirationDate < DateTime.UtcNow)
            {
                _validationToken.Clear(token.Value);

                return BadRequest(ResponseMessages.VerificationCodeExpired);
            }

            var user = _dbContext.Users
                .FirstOrDefault(x => x.Id == token.UserId);

            if (request.NewPassword.Length < AppConstants.PasswordMinLength
               || request.NewPassword.Length > AppConstants.PasswordMaxLength)
            {
                return BadRequest(ResponseMessages.PasswordLength);
            }

            user.Password = request.NewPassword;
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public ActionResult UploadPhoto(IFormFile photo)
        {
            string url = _cloudService.UploadImage(photo);

            return Ok(url);
        }
    }
}