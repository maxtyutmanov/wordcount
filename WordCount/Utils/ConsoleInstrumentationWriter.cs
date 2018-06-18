using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCount.Utils
{
    public class ConsoleInstrumentationWriter : IInstrumentationWriter
    {
        public void Write(string operation, TimeSpan runningTime)
        {
            Console.WriteLine("[Instrumentation] {0}: {1}", operation, runningTime);
        }
    }
}
