using System.Collections.Generic;
using Testbot.Enums;

namespace Testbot.Models
{
    public class Pupil
    {
        public int TotalPass { get; set; }
        public int TotalPassLessons { get; set; }
        public List<string> PassDay { get; set; }
        public string Name { get; set; }
        public Dictionary<string, List<Lessons>> PassLessons { get; set; }

        public Pupil(string name = null)
        {
            Name = name;
            TotalPass = 0;
            TotalPassLessons = 0;
            PassDay = new List<string>();
            PassLessons = new Dictionary<string, List<Lessons>>();
        }
    }
}
