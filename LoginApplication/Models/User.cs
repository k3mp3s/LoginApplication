using System;

namespace LoginApplication.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime Timeout { get; set; }
    }
}