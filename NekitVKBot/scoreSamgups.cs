using System;
using System.Collections.Generic;
using System.Text;
using VkNet.Model.GroupUpdate;
using AnyBot.Components;
using System.Threading.Tasks;

namespace NekitVKBot
{
    public class scoreSamgups : AnyBot.Components.Component
    {
        public System.Windows.Forms.WebBrowser web;
        public string html;
        public scoreSamgups()
        {
            Name = "Оценки самгупс";
            Description = "Базовый плагин";

        }
        public void Nav()
        {
            web.Navigate("https://www.samgups.ru/raspisanie/2021-2022/perviy-semestr/HTML/94.html");
            
        }
        public override void OnStart()
        {
            HtmlAgilityPack.HtmlDocument h = new HtmlAgilityPack.HtmlDocument();
            h.LoadHtml(html);
            var stroks = h.DocumentNode.SelectNodes("//table/tr");

            int index = 0;
            int index2 = 0;

            Schedule schedule = new Schedule("");

            Week week;
            foreach (var item in stroks)
            {
                
                if (index > 1 && (index != 9 && index != 10))
                {
                    Day.Lesson[] lessons = new Day.Lesson[7];
                    foreach (var item2 in item.SelectNodes("td"))
                    {
                        if (index2 > 0)
                        {
                            if (item2.InnerText.Length > 6)
                            {
                                ((GUI)this).Console(item2.InnerText);
                                lessons[index2] = new Day.Lesson(item2.InnerText);
                            }
                            else
                            {
                                lessons[index2] = null;
                            }
                        }
                        index2++;
                    } // Создание пары
                    Day day = new Day(lessons);
                    index2 = 0;
                }
                index++;
                if (index == 16) break;
            }

        }

        public override void OnEvent(GroupUpdate groupUpdate)
        {
            //Random rnd = new Random();
            //api.Messages.Send(new MessagesSendParams
            //            {
            //                RandomId = rnd.Next(),
            //                UserId = userID,
            //                Message = message,
            //            });
        }
    }
    public class Schedule
    {
        public Schedule(string HTML)
        {
            
        }
        public Week NowWeek { get; }
        public Week EvenWeek;
        public Week NotEvenWeek;
    }
    public class Week
    {
        public Week(IEnumerable<Day> days)
        {
            byte index = 0;
            foreach (var item in days)
            {
                Days[index] = item;
                index++;
            }
        }

        public Day[] Days = new Day[7];
        public Day GetDay(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return Days[6];
                case DayOfWeek.Monday:
                    return Days[0];
                case DayOfWeek.Tuesday:
                    return Days[1];
                case DayOfWeek.Wednesday:
                    return Days[2];
                case DayOfWeek.Thursday:
                    return Days[3];
                case DayOfWeek.Friday:
                    return Days[4];
                case DayOfWeek.Saturday:
                    return Days[5];
            }
            return null;
        }
        public Day NowDay { get; }

    }
    public class Day
    {
        public Day(IEnumerable<Lesson> lessons)
        {
            byte index = 0;
            foreach (var item in lessons)
            {
                Lessons[index] = item;
                index++;
            }
        }
        public Lesson[] Lessons = new Lesson[7];
        public class Lesson
        {
            public Lesson(string name)
            {
                Name = name;
            }
            public string Name;
        }
    }
}
