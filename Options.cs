using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace PMAConverter {
    internal class Options {
        public const string CurrentDirectoryMarker = "<Current directory>";

        [Option('o', "outdir",
            DefaultValue = CurrentDirectoryMarker,
            HelpText = "A directory where converted files should be placed.")]
        public string OutputDirectory { get; set; }

        [Option('c', "convert", 
            DefaultValue = ColorChange.ToPma,
            HelpText = "Color change direction, can be \"ToPma\" (converts input non-PMA images to premultiplied alpha)" +
                       " and \"FromPma\" (converts input PMA images to non-premultiplied alpha). Non case-sensitive.")]
        public ColorChange ChangeDirection { get; set; }

        [Option('p', "pngonly",
        DefaultValue = true,
        HelpText = "Changes only files with '.png' extension.")]
        public bool PngOnly { get; set; }

        [Option('r', "recursive",
            DefaultValue = false,
            HelpText = "When given directories, will look down the entire directory tree.")]
        public bool Recursive { get; set; }

        [Option('v', "verbose",
            DefaultValue = false,
            HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ValueList(typeof(List<string>))]
        public List<string> Targets { get; set; }

        [HelpOption]
        public string GetUsage() {
            var help = new HelpText {
                Heading = "pmaconv is a simple command-line utility used to change images with non-premultiplied alpha into premultiplied alpha format, and vice versa.",
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine(Environment.NewLine + "Usage examples:");
            help.AddPreOptionsLine(String.Empty);
            help.AddPreOptionsLine("Convert some images to premultiplied alpha format and place them into the given directory:");
            help.AddPreOptionsLine("pmaconv -c ToPma -o \"C:\\Directory to place converted files\" \"non-PMA.png\" \"maybe some other non-PMA.png\"");
            help.AddPreOptionsLine(String.Empty);
            help.AddPreOptionsLine("Convert images in the some directory recursively to non-premultiplied alpha format and place them into current directory:");
            help.AddPreOptionsLine("pmaconv -c FromPma -r \"E:\\Directory with PMA images to convert\"");
            help.AddOptions(this);
            return help;
        }
    }
}