using System.Collections.Generic;
using Testbot.Repositories;

namespace Testbot.Enums
{
    public class PupilsDuty
    {
        public string GenerateDutyNow(int telegramUserId)
        {
            List<string> duties = new List<string>
            {
                "Надя",
                "Марина",
                "Корчев",
                "Свириденко",
                "Нахаба",
                "Лісненко",
                "Шарапов",
                "Агеенко",
                "Поліна",
                "Таня",
                "Тарянік",
                "Шолудько",
                "Каріна",
                "Віка",
                "Даша",
                "Монаєнко",
                "Ростік",
                "Андрюшечкін",
                "Петрова",
                "Сурков",
                "Ковбаса",
                "Московець",
            };

            UserRepository users = new UserRepository();
            int currentday = users.Load().Find(e => e.TelegramUserId == telegramUserId).DutyDay;
            return duties[currentday * 2 - 1] + duties[currentday * 2 - 2];
        }
    }
}