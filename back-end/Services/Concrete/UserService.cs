using back_end.Models;
using back_end.Services.Abstract;

namespace back_end.Services
{
    public class UserService : IUserService
    {
        private readonly IDataContext _context;
        public UserService(IDataContext context)
        {
            _context=context;
        }
        public async Task Add(User user)
        {
            _context.Users?.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var itemToDelete = await _context.Users.FindAsync(id);
            if (itemToDelete == null)
            {
                throw new NullReferenceException();
            }
            _context.Users.Remove(itemToDelete);
            await _context.SaveChangesAsync();

        }

        public async Task<User> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            return user;

        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task Update(int id,User user)
        {
            var itemToUpdate = await _context.Users.FindAsync(id);
            if (itemToUpdate == null)
            {
                throw new NullReferenceException();
            }
            itemToUpdate.Id = id;
            itemToUpdate.Name = user.Name;
            itemToUpdate.Email = user.Email;
            itemToUpdate.Password = user.Password;
            itemToUpdate.Surname = user.Surname;
            itemToUpdate.Username = user.Username;

            await _context.SaveChangesAsync();
        }
    }
}
