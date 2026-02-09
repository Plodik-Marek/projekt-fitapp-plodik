using System.Security.Cryptography;
using System.Text;

namespace fitapp_plodik_MVC.Security
{
    public static class PasswordHelper
    {

        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }



        public static bool Verify(string entered, string storedHash)
        {
            
            return HashPassword(entered) == storedHash;  // zde porovnávám zadané heslo s 
        }


    }
}
