using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Testbot.Models;

namespace Testbot.Repositories
{
    public class PupilsRepository
    {
        private readonly string PathFile = "Pupils.json";
        public Pupil NewPupil(string name)
        {
            List<Pupil> pupils = new List<Pupil>();
            if (File.Exists(PathFile))
            {
                pupils = JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile));
            }
             Pupil pupil = new Pupil(name);
            pupils.Add(pupil);
            
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(pupils));
            return pupil;
        }
        public Pupil Read(string name)
        {
            List<Pupil> pupils = new List<Pupil>();
            if (File.Exists(PathFile))
            {
                pupils = JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile));
            }
            var current = pupils.Find(e => e.Name == name);

            if(current == null)
                return NewPupil(name);

            return current;
        }
        public List<Pupil> ReadAll()
        {
            List<Pupil> pupils = new List<Pupil>();
            if (File.Exists(PathFile))
            {
                pupils = JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile));
            }

            return pupils;
        }
        public void Update(Pupil pupil)
        {
            List<Pupil> pupils = new List<Pupil>();
            if (File.Exists(PathFile))
            {
                pupils = JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile));
            }

            var current = pupils.Find(e => e.Name == pupil.Name);
            if (current != null)
            {
                pupils.Remove(current);
            }

            pupils.Add(pupil);
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(pupils));
        }
    }
}
