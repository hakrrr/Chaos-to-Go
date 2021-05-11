using System;

namespace TwitchChat
{
    public class TextMessage
    {
        public DateTime Time { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        
        private TextMessage(User user, string message)
        {
            Time = DateTime.Now;
            User = user;
            Message = message;
        }
        
        public static TextMessage Create(User user, string message)
        {
            if (user == null || string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException();
            }
            
            return new TextMessage(user, message);
        }

        public override string ToString()
        {
            return $"{User.Username} ({User.Id}): {Message}";
        }
    }
}