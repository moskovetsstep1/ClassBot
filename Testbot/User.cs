using System;
using System.Collections.Generic;
using System.Text;

namespace Testbot
{
   public class User
    {
        public int TelegramUserId { get; set; }
        public StateEnum State { get; set; }
        public string Day { get; set; }
        public int DutyDay { get; set; }

        public User()
        {
            
        }
    }
}
