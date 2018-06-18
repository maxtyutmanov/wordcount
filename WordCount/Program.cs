using System;
using System.IO;
using System.Security;
using WordCount.Implementations;
using WordCount.Utils;

namespace WordCount
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments. Usage: wordcount.exe input_file_path output_file_path [/instrument]");
                return;
            }

            if (args.Length > 2 && args[2] == "/instrument")
            {
                Instrument.Initialize(new ConsoleInstrumentationWriter());
            }

            string inputPath = args[0];
            string outputPath = args[1];

            try
            {
                string inputPathError = CheckInputFilePath(inputPath);

                if (!string.IsNullOrEmpty(inputPathError))
                {
                    Console.WriteLine("Invalid input file path: {0}", inputPathError);
                    return;
                }

                string outputPathError = CheckOutputFilePath(outputPath);

                if (!string.IsNullOrEmpty(outputPathError))
                {
                    Console.WriteLine("Invalid output file path: {0}", outputPathError);
                    return;
                }

                using (var input = new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
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
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access is denied: {0}", ex.Message);
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

        private static string CheckInputFilePath(string inputPath)
        {
            var pathCheckError = CheckFilepath(inputPath);
            if (!string.IsNullOrEmpty(pathCheckError))
            {
                return pathCheckError;
            }

            if (!File.Exists(inputPath))
            {
                return $"Could not find input file {inputPath}";
            }

            return null;
        }

        private static string CheckOutputFilePath(string outputPath)
        {
            var pathCheckError = CheckFilepath(outputPath);
            if (!string.IsNullOrEmpty(pathCheckError))
            {
                return pathCheckError;
            }

            var dirName = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(dirName))
            {
                try
                {
                    //make sure output directory exists
                    Directory.CreateDirectory(dirName);
                }
                catch (IOException)
                {
                    return $"Could not create directory {dirName} for output file";
                }
            }
            return null;
        }

        private static string CheckFilepath(string filePath)
        {
            try
            {
                var fileInfo = new System.IO.FileInfo(filePath);
                if (fileInfo.Exists && (fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return $"Path {filePath} points to a directory. Specify path to the file instead.";
                }
                var dirInfo = new DirectoryInfo(filePath);
                if (dirInfo.Exists && (dirInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return $"Path {filePath} points to a directory. Specify path to the file instead.";
                }
            }
            catch (SecurityException)
            {
                return $"Access to the file {filePath} is denied";
            }
            catch (UnauthorizedAccessException)
            {
                return $"Access to the file {filePath} is denied";
            }
            catch (ArgumentException)
            {
                return $"File path {filePath} contains invalid characters";
            }
            catch (System.IO.PathTooLongException)
            {
                return $"File path {filePath} is too long";
            }
            catch (NotSupportedException)
            {
                if (filePath.Contains(":"))
                {
                    return $"File path {filePath} contains a colon in the middle of the string";
                }
                else
                {
                    return $"File path {filePath} is invalid";
                }
            }
            catch (Exception)
            {
                return $"File path {filePath} is invalid";
            }

            return null;
        }
    }
}
