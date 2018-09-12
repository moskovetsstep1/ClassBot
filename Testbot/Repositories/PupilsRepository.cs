using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Testbot.Repositories
{
    public class PupilsRepository
    {
        private readonly string PathFile = "Pupils.json";
        public void NewPupil(Pupil newPupil)
        {
            var pupils = File.Exists(PathFile) ? JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile)) : new List<Pupil>();
            pupils.Add(newPupil);
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(pupils));
        }
        public Pupil Read(string name)
        {
            var pupils = File.Exists(PathFile) ? JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile)) : new List<Pupil>();
            var currpupil = pupils.Find(e => e.Name == name);
            if(currpupil == null)
                NewPupil(new Pupil(name));
            return currpupil;
        }
        public List<Pupil> ReadAll()
        {
            var pupils = File.Exists(PathFile) ? JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile)) : new List<Pupil>();
            return pupils;
        }
        public void Update(Pupil pupil)
        {
            var pupils = File.Exists(PathFile) ? JsonConvert.DeserializeObject<List<Pupil>>(File.ReadAllText(PathFile)) : new List<Pupil>();
            var currpupil = pupils.Find(e => e.Name == pupil.Name);
            pupils.Remove(currpupil);
            currpupil.PassLessons = pupil.PassLessons;
            currpupil.PassDay = pupil.PassDay;
            currpupil.TotalPass = pupil.TotalPass;
            currpupil.TotalPassLessons = pupil.TotalPassLessons;
            pupils.Add(currpupil);
            File.WriteAllText(PathFile, JsonConvert.SerializeObject(pupils));
        }
    }
}
