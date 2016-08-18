using Justin.CLI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.NugetNew
{
    class Program
    {
        static void Main(string[] args)
        {
            new CommandProcessor<CommandArgs>(new CommandArgs(args))
                .Process(cmdArgs => 
                {
                    var pkgMapping = ReadPkgMapInfo(Args.FixPath(cmdArgs.PkgMap));
                    var result = new Dictionary<string, string>();

                    foreach (var m in pkgMapping)
                    {
                        try
                        {
                            result.Add(m.Pkg, GetPkg(m.Pkg, m.PkgDir, m.NewPkgDirFmt));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                        }
                    }

                    Output(Args.FixPath(cmdArgs.Output), result);
                });
        }

        private static string GetPkg(string pkg, string pkgDir, string dirFmt)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pkgDir);
            var dir = dirInfo.EnumerateDirectories().OrderByDescending(d => d.Name).FirstOrDefault();

            var newpkg = string.Empty;
            if (dir == null)
                Console.WriteLine($"packge {pkg} not found.");
            else
                Console.WriteLine($"{pkg} -> {newpkg = string.Format(dirFmt, dir.Name)}");

            return newpkg;
        }
        private static List<PkgMapInfo> ReadPkgMapInfo(string mappingFile)
        {
            var mapping = new List<PkgMapInfo>();

            using (StreamReader reader = new StreamReader(File.OpenRead(mappingFile)))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    var m = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (m.Length == 3)
                    {
                        mapping.Add(new PkgMapInfo
                        {
                            Pkg = m[0],
                            PkgDir = m[1],
                            NewPkgDirFmt = m[2],
                        });
                    }
                }
            }

            return mapping;
        }
        private static void Output(string output, Dictionary<string, string> mapping)
        {
            using (var writer = new StreamWriter(File.OpenWrite(output)))
            {
                foreach (var m in mapping)
                {
                    writer.WriteLine($"{m.Key}|{m.Value}");
                }
            }
        }
    }

    class PkgMapInfo
    {
        public string Pkg { get; set; }
        public string PkgDir { get; set; }
        public string NewPkgDirFmt { get; set; }
    }
}
