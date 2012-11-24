using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI
{
    public struct ProcessModule
    {
        readonly Handle m_process;
        readonly Handle m_handle;

        public ProcessModule(Handle hProcess, Handle hModule)
        {
            Contract.Requires(hProcess != Process.InvalidProcessHandle);
            // TODO

            m_process = hProcess;
            m_handle = hModule;
        }
    }
}
