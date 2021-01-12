using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sound_Space_Editor
{
    static class Options
    {
        public static List<int> color1 = new List<int> { 255, 0, 0 };
        public static List<int> color2 = new List<int> { 0, 255, 255 };
        private static void Save(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            var lines = new List<string> { "uwu" };

            File.WriteAllLines(path,lines)
        }
        private static void Load(string path)
        {
            if (!File.Exists(path))
            {
                Save(path)
            }
            else
            {
                var lines = File.ReadAllLines(path);
                
            }
        }
    }
}
