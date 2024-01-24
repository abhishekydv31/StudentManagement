using BCrypt.Net;

namespace LoginAndViewData.Utility
{
    public class PasswordEncryption
    {
        public string HashPassword(string Password) { 
            return BCrypt.Net.BCrypt.HashPassword(Password,BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}
