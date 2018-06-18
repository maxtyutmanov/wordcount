using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCount.Utils
{
    public interface IInstrumentationWriter
    {
        void Write(string operation, TimeSpan runningTime);
    }
}
