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
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Telegram.Bot;
using Telegram.Bot.Args;
using Testbot.Enums;
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
            try
            {
                Bot.SendTextMessageAsync(e.Message.From.Id, StateMachine(e.Message.Text, e.Message.From.Id));

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public static string StateMachine(string command, int telegramUserId)
        {
          //  return null;
            string pupilsname = "/end - закончить перечисление\r\n/ageenko - Агеенко\r\n/andrush - Андрюшечкін\r\n/gerasum - Герасименко\r\n/kovbasa - Ковбаса\r\n/korchev - Корчев\r\n/kuzmenk - Кузьменко\r\n/kurylen - Куриленко\r\n/lisnenk - Лісненко\r\n/monaenk - Монаєнко\r\n/moskove - Московець\r\n/nahabai - Нахаба\r\n/petrova - Петрова\r\n/rokytan - Рокитянський\r\n/svurude - Свириденко\r\n/surkovs - Сурков\r\n/stoyanp - Стоян\r\n/taranyk - Тарянік\r\n/tkachen - Ткаченко\r\n/sharapo - Шарапов\r\n/sholudk - Шолудько\r\n/shtanko - Штанько\r\n/yakoven - Яковенко";
            string lessons =
                "/end - закончить перечисление\r\n/algebra - Алгебра\r\n/biologi - Биология\r\n/chemist - Химия\r\n/english - Английский\r\n/fizikak - Физика\r\n/fizkult - Физра\r\n/geometr - Геометрия\r\n/gromads -  Громадська\r\n/ukrmova - УкрМова\r\n/ukrlitr - УкрЛит\r\n/worldli - Заруба";
            UserRepository users = new UserRepository();
            PupilsRepository pupiles = new PupilsRepository();
            var currentuser = users.Load().Find(e => e.TelegramUserId == telegramUserId);
            if (command == "/start")
            {
                users.NewUser(telegramUserId);
                return "/duty\r\n/day\r\n/skipday\r\n/skiples\r\n/infoday\r\n/send";
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
                        return "Введите дату";
                    }
                    if (command == "/skipday")
                    {
                        currentuser.State = StateEnum.PassDay;
                        users.Update(currentuser);
                        return pupilsname;
                    }
                    if (command == "/skiples")
                    {
                        currentuser.State = StateEnum.SelectPassLesson;
                        users.Update(currentuser);
                        return lessons;
                    }
                    if (command == "/infoday")
                    {
                        currentuser.State = StateEnum.InfoDay;
                        users.Update(currentuser);
                        return "Введите день";
                    }

                    if (command == "/send")
                    {
                        currentuser.State = StateEnum.SendMessType;
                        users.Update(currentuser);
                        return "Тип сообщения /day\r\n";
                    }
                    else
                    {
                        return "чет не то";
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
                        return "День изменен \r\n /duty\r\n/day\r\n/skipday\r\n/skiples\r\n/infoday\r\n/send";
                    }
                    else
                    {
                        return "Такой день уже есть";
                    }
                case StateEnum.SelectPassLesson:
                    currentuser.ChosenDay = CommandLessonToEnum(command);
                    if (currentuser.ChosenDay == Lessons.Unnamed)
                        return "нет такого урока";
                    currentuser.State = StateEnum.PassLesson;
                    users.Update(currentuser);
                    return currentuser.ChosenDay + " выбран" + '\n' + pupilsname;
                case StateEnum.PassLesson:
                    if (command == "/end")
                    {
                        currentuser.State = StateEnum.Default;
                        users.Update(currentuser);
                        return "Перечисление закончено";
                    }
                    var pupilnam = CommandUserToName(command);
                    if (pupilnam == null)
                        return "ошибка";
                    var currentPupi = pupiles.Read(pupilnam);
                    if (currentPupi.PassLessons.ContainsKey(currentuser.Day))
                        currentPupi.PassLessons[currentuser.Day].Add(currentuser.ChosenDay);
                    else
                    {
                        currentPupi.PassLessons.Add(currentuser.Day, new List<Lessons>());    
                    }

                    currentPupi.TotalPassLessons++;
                    pupiles.Update(currentPupi);
                    return pupilnam + " пропустил(a) день " + currentuser.Day;

                case StateEnum.SendMessType:
                    if (command == "/day")
                    {
                        currentuser.State = StateEnum.SelectSendMessDay;
                        users.Update(currentuser);
                        return "Напишите день";
                    }
                    break;
                case StateEnum.SelectSendMessDay:
                        var listpupils1 = pupiles.ReadAll();
                        var res1 = listpupils1.Where(e => e.PassDay.Contains(command)).Select(e => e.Name);
                        currentuser.State = StateEnum.Default;
                        users.Update(currentuser);

                        EmailServise.SendMessage($"Отсутвующие {command}", string.Join('\n', res1));
                        return "Письмо будет отправлено\r\n" + "/duty\r\n/day\r\n/skipday\r\n/skiples\r\n/infoday\r\n/send";
                case StateEnum.PassDay:
                    if (command == "/end")
                    {
                        currentuser.State = StateEnum.Default;
                        users.Update(currentuser);
                        return "Перечисление закончено \r\n/duty\r\n/day\r\n/skipday\r\n/skiples\r\n/infoday\r\n/send";
                    }
                    var pupilname = CommandUserToName(command);
                    if (pupilname == null)
                        return "ошибка";
                    var currentPupil = pupiles.Read(pupilname);
                    if(!currentPupil.PassDay.Contains(currentuser.Day))
                        currentPupil.PassDay.Add(currentuser.Day);
                    currentPupil.TotalPass++;
                    pupiles.Update(currentPupil);
                    return pupilname + " пропустил(a) день " + currentuser.Day;
                case StateEnum.InfoDay:
                    var listpupils = pupiles.ReadAll();
                  var res = listpupils.Where(e => e.PassDay.Contains(command)).Select(e => e.Name);
                    currentuser.State = StateEnum.Default;
                    users.Update(currentuser);
                    string answer =  string.Join('\n', res);
                    return answer;
            }
            return null;
        }

        private static string CommandUserToName(string command)
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

        private static Lessons CommandLessonToEnum(string command)
        {

            switch (command)
            {
                case "/algebra": return Lessons.Алгебра;
                case "/biologi": return Lessons.Биология;
                case "/chemist": return Lessons.Химия;
                case "/english": return Lessons.Английский;
                case "/fizikak": return Lessons.Физика;
                case "/fizkult": return Lessons.Физра;
                case "/geometr": return Lessons.Геометрия;
                case "/gromads": return Lessons.Громадська;
                case "/ukrmova": return Lessons.УкрМова;
                case "/ukrlitr": return Lessons.УкрЛит;
                case "/worldli": return Lessons.Заруба;
                default: return Lessons.Unnamed;
            }

        }

    }
}
