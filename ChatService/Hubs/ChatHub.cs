using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatService.Hubs
{
    public class ChatHub:Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string,UserConnection> _connections;
        public ChatHub(IDictionary<string,UserConnection> connections)
        {
            _connections = connections;
            _botUser = "MyChat Bot";
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId,out UserConnection userConnection))
            {
                _connections.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room)
                    .SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has left");
                SendConnectedUsers(userConnection.Room);
            }
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Send ressage to all users in hub
        /// </summary>
        /// <param name="message">message received from chat</param>
        /// <returns></returns>
        public async Task SendMessage(string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId,out UserConnection userConnection))
            {
                // THIS SECTION IF FOR TESTING PURPOSE ONLY
                if (message[0] == '-')
                {
                    int roleid = 0;
                    int statusid = 0;

                    if (message.Split(" ")[1].ToLower() == "mafia") roleid = 2;
                    else if (message.Split(" ")[1].ToLower() == "default") roleid = 3;

                    if (message.Split(" ")[2].ToLower() == "murdered") statusid = 3;
                    else if (message.Split(" ")[2].ToLower() == "voted" && message.Split(" ")[3].ToLower() == "out") statusid = 2;

                    if (roleid == 0 || statusid == 0)
                        await InformPlayerStatus(roleid, statusid);
                    else
                    {
                        string msg = await InformPlayerStatus(roleid, statusid);
                        await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, msg);
                    }
                }
                // THIS SECTION IF FOR TESTING PURPOSE ONLY

                else
                    await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message);
            }
        }
        public async Task JoinRoom(UserConnection userConnection )
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
            _connections[Context.ConnectionId] = userConnection;
            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage",_botUser,
                $"{userConnection.User} has joined {userConnection.Room}");
            await SendConnectedUsers(userConnection.Room);
        }
        
        public Task SendConnectedUsers(string room)
        {
            var users = _connections.Values
                .Where(c => c.Room == room)
                .Select(c => c.User);
            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }




        /// <summary>
        /// Get information about player status(what happened to player), considering the role of the player, who performed action(roleId)
        /// </summary>
        /// <param name="roleId">Id of the role, that performed action on player</param>
        /// <param name="statusId">Id of the action performed on player</param>
        /// <returns>Random message according to role id and status id from the database</returns>
        public async Task<string> InformPlayerStatus(int roleId, int statusId)
        {
            Bots.BotModels.PlayerStatusInformerBot bot = new Bots.BotModels.PlayerStatusInformerBot();
            return await bot.GetPlayerStatusMessage(roleId, statusId);
        }

    }
}
