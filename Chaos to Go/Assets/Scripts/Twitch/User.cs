using System;

namespace TwitchChat
{
    public class User
    {
        public Guid Id { get; set; }
        
        public DateTime LastActive { get; set; }
        
        public string Username { get; set; }
        
        public User(string normalizedUsername)
        {
            Id = Guid.NewGuid();
            LastActive = DateTime.Now;
            Username = normalizedUsername;
        }

        public bool Equals(User other)
        {
            return other.Id == Id;
        }

        public void Update()
        {
            LastActive = DateTime.Now;
        }
    }
}