using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.FileSync
{
    class Program
    {
        static void Main(string[] args)
        {
            new CommandProcessor<CommandArgs>(new CommandArgs(args))
                .Process(cmdArgs => 
                {
                    var map_file = Args.FixPath(cmdArgs.MapFile);
                    var dirsMapping = Utils.ReadMapping(map_file, '|');
                    var webClient = CreateWebClient(cmdArgs.Protocol, cmdArgs.User, cmdArgs.Pwd);
                    var serverRoot = $"{cmdArgs.Protocol}://{cmdArgs.Server}";

                    foreach (var entry in dirsMapping)
                    {
                        var localDir = Args.FixPath(entry.Key);
                        foreach (var f in new DirectoryInfo(localDir).EnumerateFiles())
                        {
                            var remote = serverRoot + "/" + entry.Value + "/" + f.Name;

                            try
                            {
                                DownloadFileFtp(remote, f.FullName, webClient);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                    }
                });
        }

        private static WebClient CreateWebClient(string protocol, string userId, string password)
        {
            switch (protocol)
            {
                case CommandArgs.PROTOCOL_FTP:
                    return new FtpClient(userId, password);

                case CommandArgs.PROTOCOL_HTTP:
                    return new HttpClient(userId, password);

                case CommandArgs.PROTOCOL_FILE:
                    return new FileClient(userId, password);

                default:
                    throw new ArgumentException("invalid protocol " + protocol);
            }

        }
        private static void DownloadFileFtp(string remote, string target, WebClient webClient)
        {
            var char_p = "".PadRight(50, '*').ToArray();
            var char_n = "".PadRight(50, ' ').ToArray();

            Console.WriteLine(remote);
            Console.Write("connecting...");

            webClient.Download(remote, target, (p, t) =>
            {
                Console.CursorLeft = 0;

                var prog = Math.Round((p * 1.0 / t), 2);
                var l_p = (int)(char_p.Length * prog);
                var l_n = char_p.Length - l_p;

                Console.Write(char_p, 0, l_p);
                Console.Write(char_n, 0, l_n);

                if(prog == 1.0)
                {
                    Console.Write(" " + (prog * 100).ToString() + "% Ok".PadRight(10, ' '));
                }
                else
                {
                    Console.Write(" " + (prog * 100).ToString() + "%".PadRight(10, ' '));
                }
            });

            Console.WriteLine();
        }
        
    }
}
