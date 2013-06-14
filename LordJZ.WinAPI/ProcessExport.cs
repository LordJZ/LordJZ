using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI
{
    public struct ProcessExport
    {
        public Process Process { get; set; }
        public ProcessModule Module { get; set; }
        public int Ordinal { get; set; }
        public string Name { get; set; }
        public IntPtr Address { get; set; }
    }
}
