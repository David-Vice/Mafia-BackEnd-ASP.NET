using System.Linq;
using System.Security.Cryptography;

namespace back_end.Security
{
    public class PasswordHasher:IPasswordHasher
    {
        public void CreatePassswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac= new HMACSHA512() )
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


            }
        }
        public bool VerifyPassswordHash(string password, byte[] passwordHash,byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                
                var computedHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
