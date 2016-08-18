using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.NugetNew
{
    class CommandArgs : Args
    {
        private const string ARG_PKG_MAP = "-m";
        private const string ARG_OUTPUT = "-o";

        public string PkgMap
        {
            get
            {
                var value = GetArg(ARG_PKG_MAP);

                if (value == null)
                    SetArg(ARG_PKG_MAP, value = "map.txt");

                return value;
            }
        }
        public string Output
        {
            get
            {
                var value = GetArg(ARG_OUTPUT);

                if (value == null)
                    SetArg(ARG_OUTPUT, value = "nugetnew_pkg_mapping.txt");

                return value;
            }
        }

        public CommandArgs(params string[] args) 
            : base(args) { }

        protected override string GetArgsHelpInfoInternal()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(ArgUsageWithDefault(ARG_PKG_MAP, "the map file specifies the mapping of package and directory", "map.txt"));
            sb.AppendLine(ArgUsageWithDefault(ARG_OUTPUT, "the path of output file", "pkg_mapping.txt"));

            return sb.ToString();
        }
        
    }
}
