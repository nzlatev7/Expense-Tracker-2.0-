using Expense_Tracker_2._0.Models.DB;
using Expense_Tracker_2._0.Services.Interfaces;
using System.Security.Cryptography;

namespace Expense_Tracker_2._0.Services
{
    public class ValidationTokenService : IValidationToken
    {
        private readonly ExpenseTrackerDbContext _dbContext;

        public ValidationTokenService(ExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }   

        public ValidationToken Generate(int userId)
        {
            string token = CreateToken();

            while (_dbContext.ValidationTokens.Any(x => x.Value == token))
            {
                token = CreateToken();
            }

            DateTime expirationDate = DateTime.UtcNow.AddSeconds(120);

            ValidationToken validationToken = new ValidationToken
            {
                Value = token,
                ExpirationDate = expirationDate,
                UserId = userId,
                AttemptsCount = 1,
            };

            _dbContext.ValidationTokens.Add(validationToken);
            _dbContext.SaveChanges();

            return validationToken;
        }

        public void Clear(string tokenValue)
        {
            var validationToken = _dbContext.ValidationTokens
                .FirstOrDefault(t => t.Value == tokenValue);

            if (validationToken != null)
            {
                _dbContext.ValidationTokens.Remove(validationToken);
                _dbContext.SaveChanges();
            }
        } 

        public ValidationToken Resend(int userId)
        {
            var existingToken = _dbContext.ValidationTokens
                .FirstOrDefault(x => x.UserId == userId);

            if (existingToken != null)
            {
                Clear(existingToken.Value);
            }

            return Generate(userId);
        }

        private string CreateToken()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[2];
                rng.GetBytes(bytes);

                // Convert the random bytes to a 16-bit unsigned integer
                ushort randomValue = BitConverter.ToUInt16(bytes, 0);

                // Generate a 4-digit security code
                string token = (randomValue % 10000).ToString("D4");

                return token;
            }
        }
    }
}
