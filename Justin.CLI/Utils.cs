using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI
{
    public static class Utils
    {
        public static Dictionary<string, string> ReadMapping(string mappingFile, char split = ' ')
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(File.OpenRead(mappingFile)))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    var m = line.Split(new char[] { split }, StringSplitOptions.RemoveEmptyEntries);

                    if (m.Length == 2)
                    {
                        mapping[m[0]] = m[1];
                    }
                }
            }

            return mapping;
        }
    }
}
