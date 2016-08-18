using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI
{
    public class CommandProcessor<T> where T : Args
    {
        private T m_cmdArgs;

        public CommandProcessor(T cmdArgs)
        {
            m_cmdArgs = cmdArgs;
        }

        public void Process(Action<T> proc)
        {
            if (m_cmdArgs.Help != null)
            {// -? 参数处理
                Console.WriteLine(m_cmdArgs.GetArgsHelpInfo());
            }
            else
            {
                var info = m_cmdArgs.Validate();
                if (!string.IsNullOrEmpty(info))
                { // 参数验证
                    Console.WriteLine(info);
                }
                else
                {
                    proc(m_cmdArgs as T);
                }
            }
        }
    }
}
