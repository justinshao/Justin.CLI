using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI
{
    public class Args
    {
        private Dictionary<string, string> m_args = new Dictionary<string, string>();

        private const string ARG_HELP = "-?";
        private const string ARG_CONF = "-f";

        public string Help
        {
            get { return GetArg(ARG_HELP); }
        }
        public string Conf
        {
            get { return GetArg(ARG_CONF); }
        }

        public Args(params string[] args)
        {
            if (args.Length == 0)
                return;

            if (args[0] == ARG_HELP)
            {
                SetArg(ARG_HELP, string.Empty);
            }
            else
            {
                for (int i = 0; i < args.Length; i += 2)
                {
                    if (i + 1 < args.Length)
                    {
                        m_args[args[i]] = args[i + 1];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (Conf != null)
                ReadFromFile(FixPath(Conf));
        }

        protected string GetArg(string name)
        {
            if (m_args.ContainsKey(name))
            {
                return m_args[name];
            }
            else
            {
                return null;
            }
        }
        protected void SetArg(string name, string value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "argument " + name + " can not be specified null.");

            m_args[name] = value;
        }

        private void ReadFromFile(string confFile)
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(confFile)))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    var name_value = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (name_value.Length == 2)
                    {
                        if(!m_args.ContainsKey(name_value[0]))
                        {
                            m_args[name_value[0]] = name_value[1];
                        }
                    }
                }
            }
        }
        
        public string GetArgsHelpInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Command Args:");

            sb.AppendLine(ArgUsage(ARG_HELP, "output options usage"));
            sb.AppendLine(ArgUsage(ARG_CONF, "the file which specify all the command args"));

            sb.Append(GetArgsHelpInfoInternal());

            return sb.ToString();
        }

        protected virtual string GetArgsHelpInfoInternal()
        {
            return string.Empty;
        }
        public virtual string Validate()
        {
            return string.Empty;
        }

        public static string FixPath(string path)
        {
            return path.IndexOf(":") > 0 ? path : 
                Path.Combine(Environment.CurrentDirectory, path);
        }
        protected static string ArgRequired(string name)
        {
            return $"the arg '{name}' is required. type '-?' for help.";
        }

        protected static string ArgUsage(string name, string usage, bool required = false)
        {
            return string.Format(required ? "\t{0} {1}." : "\t{0} <{1}.>", name, usage);
        }
        protected static string ArgUsageWithDefault(string name, string usage, string defVal)
        {
            return string.Format("\t{0} {1}\r\n\t   -default: {2}", name, usage, defVal);
        }
        protected static string ArgOptions(string name, params string[] options)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("the argument {0} must be one in (", name);
            for (int i = 0; i < options.Length; i++)
            {
                if(i == options.Length - 1)
                {
                    sb.Append(options[i]);
                }
                else
                {
                    sb.Append(options[i]).Append(',');
                }
            }
            sb.Append(")");

            return sb.ToString();
        }
    }
}
