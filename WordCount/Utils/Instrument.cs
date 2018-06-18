using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCount.Utils
{
    public static class Instrument
    {
        private static IInstrumentationWriter _writer;

        public static void Initialize(IInstrumentationWriter writer)
        {
            _writer = writer;
        }

        public static void This(Action action, string operation)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();

            if (_writer != null)
            {
                _writer.Write(operation, sw.Elapsed);
            }
        }
    }
}
