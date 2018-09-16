using Testbot.Enums;

namespace Testbot.Models
{
   public class User
    {
        public int TelegramUserId { get; set; }
        public StateEnum State { get; set; }
        public string Day { get; set; }
        public int DutyDay { get; set; }
        public Lessons ChosenDay { get; set; }

        public User()
        {
            
        }

        public User(int telegramUserId, StateEnum state, int dutyDay)
        {
            TelegramUserId = telegramUserId;
            State = state;
            DutyDay = dutyDay;
            ChosenDay = Lessons.Unnamed;
        }
    }
}
