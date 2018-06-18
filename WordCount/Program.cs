using System;
using System.IO;
using WordCount.Implementations;

namespace WordCount
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments. Usage: wordcount.exe input_file_path output_file_path");
                return;
            }

            try
            {
                using (var input = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var output = new FileStream(args[1], FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var inputReader = new InputReader();
                    var inputPartitioner = new ByFirstCharPartitioner();
                    var freqDictBuilder = new InMemoryFreqDictionaryBuilder();
                    var outputWriter = new OutputWriter();

                    var facade = new Facade(inputReader, inputPartitioner, freqDictBuilder, outputWriter);
                    facade.Run(input, output);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File {0} not found", ex.FileName);
            }
            catch (IOException ex)
            {
                Console.WriteLine("IO error: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error {0}", ex);
            }
        }
    }
}
