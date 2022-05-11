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

        public async Task SendMessage(string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId,out UserConnection userConnection))
            {
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

    }
}
