//using System;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Telegram.Bot.Args;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.InlineQueryResults;
//using Telegram.Bot.Types.ReplyMarkups;

//namespace Testbot
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var Bot = new MazeBot("557358914:AAE03Faw9-BwKFygJHFMl530FiGH9sPvB6Y");
//            Console.WriteLine("Hello World!");
//        }
//    }
//}

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Testbot.Repositories;

namespace Testbot
{
    public static class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("574977062:AAHC1xLdYfEtZ8EYQYuprhAi3DJYDhcgeGw");

        public static void Main(string[] args)
        {
            
            Bot.OnMessage += BotOnMessageReceived;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            Bot.SendTextMessageAsync(e.Message.From.Id, StateMachine(e.Message.Text, e.Message.From.Id));
        }

        public static string StateMachine(string command, int telegramUserId)
        {
            string s = "/end - закончить перечисление\r\n/ageenko - Агеенко\r\n/andrush - Андрюшечкін\r\n/gerasum - Герасименко\r\n/kovbasa - Ковбаса\r\n/korchev - Корчев\r\n/kuzmenk - Кузьменко\r\n/kurylen - Куриленко\r\n/lisnenk - Лісненко\r\n/monaenk - Монаєнко\r\n/moskove - Московець\r\n/nahabai - Нахаба\r\n/petrova - Петрова\r\n/rokytan - Рокитянський\r\n/svurude - Свириденко\r\n/surkovs - Сурков\r\n/stoyanp - Стоян\r\n/taranyk - Тарянік\r\n/tkachen - Ткаченко\r\n/sharapo - Шарапов\r\n/sholudk - Шолудько\r\n/shtanko - Штанько\r\n/yakoven - Яковенко";
            UserRepository users = new UserRepository();
            PupilsRepository pupiles = new PupilsRepository();
            var currentuser = users.Load().Find(e => e.TelegramUserId == telegramUserId);
            if (command == "/start")
            {
                users.NewUser(telegramUserId);
                return "Зарегестрированы";
            }

            switch (currentuser.State)
            {
                case StateEnum.Default:

                    if (command == "/duty")
                    {
                        PupilsDuty pupils = new PupilsDuty();
                        return pupils.GenerateDutyNow(telegramUserId);
                    }

                    if (command == "/day")
                    {
                        currentuser.State = StateEnum.SelectNowDay;
                        users.Update(currentuser);
                        return "Гуд";
                    }
                    if (command == "/skipday")
                    {
                        currentuser.State = StateEnum.PassDay;
                        users.Update(currentuser);
                        return s;
                    }
                    if (command == "/infoday")
                    {
                        currentuser.State = StateEnum.InfoDay;
                        users.Update(currentuser);
                        return "Введите день";
                    }
                    else
                    {
                        return "";
                    }
                case StateEnum.SelectNowDay:
                    if (currentuser.Day != command)
                    {
                        currentuser.Day = command;
                        currentuser.DutyDay++;
                        if (currentuser.DutyDay == 12)
                            currentuser.DutyDay = 1;
                        currentuser.State = StateEnum.Default;
                        users.Update(currentuser);
                        return "гуд";
                    }
                    else
                    {
                        return "чет не то";
                    }
                case StateEnum.PassDay:
                    if (command == "/end")
                    {
                        currentuser.State = StateEnum.Default;
                        users.Update(currentuser);
                        return "перечисление закончено";
                    }
                    var pupilname = CommandToEnum(command);
                    if (pupilname == null)
                        return "ошибка";
                    var currentPupil = pupiles.Read(pupilname);
                    if(!currentPupil.PassDay.Contains(currentuser.Day))
                        currentPupil.PassDay.Add(currentuser.Day);
                    pupiles.Update(currentPupil);
                    return pupilname + " пропустил(a) день " + currentuser.Day;
                case StateEnum.InfoDay:
                    var listpupils = pupiles.ReadAll();
                  var res = listpupils.Where(e => e.PassDay.Contains(command)).Select(e => e.Name);
                    return  string.Join('\n', res);
            }
            return null;
        }

        private static string CommandToEnum(string command)
        {
            switch (command)
            {
                case "/ageenko": return "Агеенко";
                case "/andrush": return "Андрюшечкін";
                case "/gerasum": return "Герасименко";
                case "/kovbasa": return "Ковбаса";
                case "/korchev": return "Корчев";
                case "/kuzmenk": return "Кузьменко";
                case "/kurylen": return "Куриленко";
                case "/lisnenk": return "Лісненко";
                case "/monaenk": return "Монаєнко";
                case "/moskove": return "Московець";
                case "/nahabai": return "Нахаба";
                case "/petrova": return "Петрова";
                case "/rokytan": return "Рокитянський";
                case "/svurude": return "Свириденко";
                case "/surkovs": return "Сурков";
                case "/stoyanp": return "Стоян";
                case "/taranyk": return "Тарянік";
                case "/tkachen": return "Ткаченко";
                case "/sharapo": return "Шарапов";
                case "/sholudk": return "Шолудько";
                case "/shtanko": return "Штанько";
                case "/yakoven": return "Яковенко";
            }
            return null;
        }
    }
}
