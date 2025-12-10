using System;

namespace TaskManager.Models
{
    public class LoginLog
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string IpAddress { get; set; }
    }
}
