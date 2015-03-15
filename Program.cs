using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace PMAConverter {
    internal static class Program {
        private static bool _verbose;
        private static readonly object _consoleLock = new object(); // Needed for correct console font color change.

        static void Main(string[] args) {
            // Parse command line arguments:
            var options = new Options();
            Parser.Default.ParseArgumentsStrict(args, options, () => {
                Environment.Exit(Parser.DefaultExitCodeFail);
            });

            _verbose = options.Verbose;
            if (options.Targets.Count == 0) {
                Error("No target files or directories passed in arguments. Use --help if needed.");
                Environment.Exit(Parser.DefaultExitCodeFail);
            }
      
            InitializeOutputDir(options);
            // Explain what will be done if verbose is enabled:
            Info("Output directory is set to '" + options.OutputDirectory + "'.");
            if (options.ChangeDirection == ColorChange.ToPma) {
                Info("Images will be changed from non-premultiplied alpha to premultiplied alpha.");
            } else if (options.ChangeDirection == ColorChange.FromPma) {
                Info("Images will be changed from premultiplied alpha to non-premultiplied alpha.");
            }
            Info(options.PngOnly ? "Only .png files will be changed." : "Every file will be tried for a change.");
            Info(options.Recursive ? "Passed directories will be inspected recursively." : 
                "Passed directories will be inspected on a top level only.");


            // Start working here:
            foreach (var target in options.Targets) {
                // Unbound argument can be either a file to convert or a directory containing files to convert.
                if (File.Exists(target)) {
                    ProcessFileTarget(target, options);
                } else if (Directory.Exists(target)) {
                    ProcessDirectoryTarget(target, options);
                } else { Error("'" + target + "' does not exist."); }
            }

            Info("Exiting...");
        }

        /// <summary> Converts the specified image file according to the options. </summary>
        private static void ProcessFileTarget(string filename, Options options) {
            // Check if it is .png. Of course, it would be more correct (and slow) to open stream and check file format.
            var fi = new FileInfo(filename);
            if (options.PngOnly && fi.Extension != ".png") {
                Error("File does not end with '.png', skipping.");
                return;
            }
            
            // Actual conversion:
            Info("Converting '" + filename + "'...");
            try {
                string output = Path.Combine(options.OutputDirectory, fi.Name);
                var factory = new PmaImageFactory().Load(filename).ChangePremultipliedAlpha(options.ChangeDirection);
                Info("Writing to '" + output +"'...");
                factory.Save(output);
                Success("File '" + filename + "' done! ");
            } catch (Exception ex) { Error(filename + ": " + ex.Message); }
        }

        /// <summary> Converts all files in the given directory. </summary>
        private static void ProcessDirectoryTarget(string directoryName, Options options) {
            try {
                Info("Converting everything in the '" + directoryName + "'...");

                string pattern = options.PngOnly ? "*.png" : "*.*";
                var search = options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var files = Directory.EnumerateFiles(directoryName, pattern, search).ToArray();
                Info("Total " + files.Length + " files found.");

                Parallel.ForEach(files, file => ProcessFileTarget(file, options));

                Success("Directory '" + directoryName + "' done!");
            } catch (Exception ex) { Error(directoryName + ": " + ex.Message); }
        }

        /// <summary> Initializes and creates output directory if needed. </summary>
        private static void InitializeOutputDir(Options options) {
            
            if (String.IsNullOrEmpty(options.OutputDirectory)
                || options.OutputDirectory == Options.CurrentDirectoryMarker) {
                options.OutputDirectory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(options.OutputDirectory)) {
                try {
                    Directory.CreateDirectory(options.OutputDirectory);
                } catch (Exception ex) {
                    Error(ex.Message);
                    Environment.Exit(Parser.DefaultExitCodeFail);
                }
            }
        }

        private static void Info(string text) {
            if (_verbose) {
                lock (_consoleLock) {
                    Console.WriteLine(text);
                }
            }
        }

        private static void Success(string text) {
            lock (_consoleLock) {
                Console.WriteLine(text);
            }
        }

        private static void Error(string text) {
            lock (_consoleLock) {
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ForegroundColor = prevColor;
            }
        }
    }
}
