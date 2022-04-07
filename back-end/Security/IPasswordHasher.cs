namespace back_end.Security
{
    public interface IPasswordHasher
    {
        public void CreatePassswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPassswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

    }
}
