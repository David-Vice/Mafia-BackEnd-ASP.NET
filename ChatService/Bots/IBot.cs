using System.Threading.Tasks;

namespace ChatService.Bots
{
    public interface IBot
    {
        Task<string> GetPlayerStatusMessage(int roleId, int statusId);
    }
}
