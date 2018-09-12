using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Testbot.Enums;
using Testbot.Models;

namespace Testbot.Repositories
{
   public class UserRepository
    {
        private readonly string PathFile = "Users.json";
        public void NewUser(int telegramUserId)
        {
            List<User> users = new List<User>();
            if (File.Exists(PathFile))
            {
                users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(PathFile));
            }

            users.Add(new User(telegramUserId, StateEnum.Default, 1));

            File.WriteAllText(PathFile, JsonConvert.SerializeObject(users));
        }
        public List<User> Load()
        {
            List<User> users = new List<User>();
            if (File.Exists(PathFile))
            {
                users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(PathFile));
            }
            return users;
        }
        public void Update(User user)
        {
            List<User> users = new List<User>();
            if (File.Exists(PathFile))
            {
                users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(PathFile));
            }

            var removedUser = users.Find(e => e.TelegramUserId == user.TelegramUserId);
            if (removedUser != null)
            {
                users.Remove(removedUser);
            }
            users.Add(user);
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(users));
        }
    }
}
