using back_end.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface ISessionService
    {
        Task<Session> Get(int id);
        Task<IEnumerable<Session>> GetOpenSessions();
        Task<IEnumerable<Session>> GetAll();
        Task Add(Session session);
        Task Delete(int id);
        Task EndSession(int id);
    }
}