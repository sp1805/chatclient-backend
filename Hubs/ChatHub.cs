using ChatServer.Models;
using ChatServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        ManageUserInfo manageUserInfo;

        public ChatHub(ManageUserInfo userInfo)
        {
            manageUserInfo = userInfo;
        }

        // When a new User Connects
        public override async Task OnConnectedAsync()
        {
            // Send message to joined client
            await Clients.Caller.SendAsync("ReceiveJoinMessage", DateTimeOffset.UtcNow);

            // Send message to all client
            await base.OnConnectedAsync();
        }

        public async Task UserJoinMessage(string userName, string groupName = "General")
        {
            manageUserInfo.AddUserInfo(userName, DateTimeOffset.UtcNow, Context.ConnectionId, groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            // Checks if the user is not in group then send the notification to all else send to the group members
            if (groupName == null || String.IsNullOrEmpty(groupName))
            {
                await Clients.All.SendAsync("AddNewUser", userName, DateTimeOffset.UtcNow);
            }
            else
            {
                await Clients.Group(groupName).SendAsync("AddNewUser", userName, DateTimeOffset.UtcNow);
            }
        }

        // When a user disconnects
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var UserDisconnected = manageUserInfo.AllUsers.Find(user => user.ConnectionId == Context.ConnectionId);

            // Checks if the username is not null
            if (UserDisconnected.UserName != null)
            {
                // checks if the user is not associated to any group then send the notification to all else send in the group
                if (UserDisconnected.GroupName == null || String.IsNullOrEmpty(UserDisconnected.GroupName))
                    await Clients.All.SendAsync("ReceiveDisconnectedMessage", UserDisconnected.UserName, UserDisconnected.Time);
                else
                    await Clients.Group(UserDisconnected.GroupName).SendAsync("ReceiveDisconnectedMessage", UserDisconnected.UserName, UserDisconnected.Time);
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Gets called when client invoke SendMessage event
        public async Task SendMessage(string name, string text)
        {

            var UserDisconnected = manageUserInfo.AllUsers.Find(user => user.ConnectionId == Context.ConnectionId);
            var message = new ChatMessage { SenderName = name, Text = text, SendAt = DateTimeOffset.UtcNow };

            // checks if the user is not associated to any group then send the message to all else send in the group
            if (UserDisconnected.GroupName == null || String.IsNullOrEmpty(UserDisconnected.GroupName))
            {
                // Sending all clients message -> Broadcasting
                await Clients.All.SendAsync("ReceiveMessage", message.SenderName, message.SendAt, message.Text);
            }
            else
            {
                await Clients.Group(UserDisconnected.GroupName).SendAsync("ReceiveMessage", message.SenderName, message.SendAt, message.Text);
            }
        }
    }
}
