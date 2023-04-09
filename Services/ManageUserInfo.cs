using ChatServer.Models;

namespace ChatServer.Services
{
    public class ManageUserInfo
    {
        public List<UserInfo> AllUsers = new List<UserInfo>();

        public void AddUserInfo(string name, DateTimeOffset time, string connectionId, string? groupName = null)
        {
            UserInfo currentUser = new UserInfo()
            {
                UserName = name,
                Time = time,
                Status = true,
                ConnectionId = connectionId,
                GroupName = groupName
            };
            AllUsers.Add(currentUser);
        }

        public void RemoveUserInfo(string connectionId)
        {
            UserInfo currentUser = AllUsers.Find(user => user.ConnectionId == connectionId);
            currentUser.Time = DateTimeOffset.Now;
            currentUser.Status = false;
        }


    }
}
