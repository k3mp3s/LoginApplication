using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LoginApplication.Models
{
    public class UserRepository
    {
        private static Dictionary<String, User> _users;

        public static Dictionary<String, User> GetUsers(String path)
        {
            if (_users == null)
            {
                _users = new Dictionary<String, User>();

                String[] lines = File.ReadAllLines(path);

                foreach (String line in lines)
                {
                    String[] userParams = line.Split(';');

                    User user = new User();
                    user.Username = userParams[0];
                    user.Password = userParams[1];
                    user.Email = userParams[2];

                    _users.Add(user.Username, user);
                }
            }

            return _users;
        }

        public static User GetUser(String username)
        {
            User user;

            if ((_users != null) && (username != null))
            {
                if (_users.TryGetValue(username, out user))
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}