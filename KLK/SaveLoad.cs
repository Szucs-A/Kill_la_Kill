using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Serialization;
using System.IO;

namespace KLK
{
    public class SaveLoad
    {
        public static void Save(int s)
        {
            JObject highscores = new JObject(
               new JProperty("Score", s));

            File.WriteAllText("Highscore.json", highscores.ToString());

            // write JSON directly to a file
            using (StreamWriter file = File.CreateText("Highscore.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                highscores.WriteTo(writer);
            }
        }

        public static void Load()
        {
            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText("Highscore.json"));

                // read JSON directly from a file
                using (StreamReader file = File.OpenText("Highscore.json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o2 = (JObject)JToken.ReadFrom(reader);
                    int score = (int)o2.Property("Score");
                    Console.WriteLine(score);
                }
            }
            catch (FileNotFoundException e)
            {

            }
            
        }
    }
}
