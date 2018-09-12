using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Testbot.Repositories
{
   public class UserRepository
    {
        private readonly string PathFile = "Users.json";
        public void NewUser(int telegramUserId)
        {
            var users = File.Exists(PathFile) ? JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(PathFile)) : new List<User>();
            users.Add(new User
            {
                TelegramUserId = telegramUserId,
                State = StateEnum.Default,
                DutyDay = 1,
            });
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(users));
        }
        public List<User> Load()
        {
            return  File.Exists(PathFile) ? JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(PathFile)) : new List<User>();
        }
        public void Update(User user)
        {
            var users = File.Exists(PathFile) ? JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(PathFile)) : new List<User>();
            users.Remove(users.Find(e => e.TelegramUserId == user.TelegramUserId));
            users.Add(user);
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(users));
        }
    }
}
