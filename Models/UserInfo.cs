namespace ChatServer.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public DateTimeOffset Time { get; set; }
        public bool Status { get; set; }
        public string ConnectionId { get; set; }
        public string? GroupName { get; set; }
    }
}
