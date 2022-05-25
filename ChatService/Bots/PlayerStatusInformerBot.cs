using System.Net.Http;
using System.Threading.Tasks;

namespace ChatService.Bots
{
    public class PlayerStatusInformerBot : IBot
    {
        public async Task<string> GetPlayerStatusMessage(int roleId, int statusId)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:44313/api/BotResponces/GetResponceToAction?roleId={roleId}&palyerInGameStatusId={statusId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return "no result";
        }

    }
}
