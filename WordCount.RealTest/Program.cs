using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordCount.Implementations;

namespace WordCount.RealTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //WithRealTextFile();
            WithHugeGeneratedTextFile(128 * 1024 * 1024);   //~ 128 MB
            
            Console.ReadLine();
        }

        static void WithHugeGeneratedTextFile(int lengthInChars)
        {
            var rusChars = "абвгдежзийклмнопрстуфхцчшщьъэюя    \n\n\n";
            var random = new Random();

            var inputPath = @"generated.txt";
            var outputPath = @"generated_wordcount.txt";

            using (var file = new FileStream(inputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(file, Encoding.GetEncoding("windows-1251")))
            {
                for (int i = 0; i < lengthInChars; i++)
                {
                    writer.Write(rusChars[random.Next(rusChars.Length)]);
                }
            }

            Console.WriteLine("Finished generating file");

            var sw = Stopwatch.StartNew();
            WordCount.Program.Main(new[] { inputPath, outputPath });
            sw.Stop();
            Console.WriteLine("Finished in {0}", sw.Elapsed);
        }

        static void WithRealTextFile()
        {
            var inputPath = @"Tihiy_Don_p.txt";
            var outputPath = @"Tihiy_Don_wordcount.txt";

            var sw = Stopwatch.StartNew();
            WordCount.Program.Main(new[] { inputPath, outputPath });
            sw.Stop();
            Console.WriteLine("Finished in {0}", sw.Elapsed);
        }
    }
}
