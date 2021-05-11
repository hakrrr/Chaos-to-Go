using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TwitchChat
{
    public class UserManager : Singleton<UserManager>
    {
        [SerializeField] private int userTimeoutInSeconds = 120;
        
        private List<User> _users;

        public int UserCount => _users.Count;
        
        protected override void Awake()
        {
            base.Awake();
            _users = new List<User>();
        }

        private void Update()
        {
            var inactiveUsers = _users
                .Where(user => (DateTime.Now - user.LastActive).Seconds >= userTimeoutInSeconds)
                .ToList();
            
            foreach (var user in inactiveUsers)
            {
                _users.Remove(user);
                Debug.Log($"User {user.Username} was inactive for over {userTimeoutInSeconds} seconds!");
            }
        }

        public bool UserExists(string username)
        {
            var normalizedUsername = GetNormalizedUsername(username);
            return _users
                .Any(user => user.Username.Equals(normalizedUsername));
        }

        public User RegisterOrUpdateUser(string username)
        {
            var normalizedUsername = GetNormalizedUsername(username);
            var existingUser = _users
                .FirstOrDefault(user => user.Username.Equals(normalizedUsername));

            if (existingUser == null)
            {
                var newUser = new User(normalizedUsername);
                _users.Add(newUser);
                return newUser;
            }

            existingUser.Update();
            return existingUser;
        }

        private string GetNormalizedUsername(string username)
        {
            return username.ToLower();
        }

        public string GetRandomUserName()
        {
            if (UserCount == 0)
                return "CommandBridge";
            
            var index = Random.Range(0, UserCount);
            var user = _users[index];
            return user.Username;
        }
    }
}
