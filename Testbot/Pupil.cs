using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using Testbot.Repositories;

namespace Testbot
{
    public class Pupil
    {
        public int TotalPass { get; set; }
        public int TotalPassLessons { get; set; }
        public List<string> PassDay { get; set; }
        public string Name { get; set; }
        public Dictionary<string, List<Lessons>> PassLessons { get; set; }

        public Pupil()
        {
            TotalPass = 0;
            TotalPassLessons = 0;
            PassDay = new List<string>();
            PassLessons = new Dictionary<string, List<Lessons>>();
            PupilsRepository a = new PupilsRepository();
            a.NewPupil(this);
        }
        public Pupil(string name)
        {
            Name = name;
            TotalPass = 0;
            TotalPassLessons = 0;
            PassDay = new List<string>();
            PassLessons = new Dictionary<string, List<Lessons>>();
          
        }
    }
}
