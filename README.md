# pmaconv

pmaconv is a simple command-line utility used to change images with non-premultiplied alpha into premultiplied alpha format, and vice versa.


### Usage examples

Convert some images to premultiplied alpha format and place them into the given directory:

    pmaconv -c ToPma -o "C:\Directory to place converted files" "non-PMA.png" "maybe some other non-PMA.png"

Convert images in the some directory non-recursively to non-premultiplied alpha format and place them into current directory:

    pmaconv -c FromPma -r false "E:\Directory with PMA images to convert"


### Options

    -o, --outdir       (Default: <Current directory>) A directory where converted files should be placed.

    -c, --convert      (Default: ToPma) Color change direction, can be 
                       "ToPma" (converts input non-PMA images to premultiplied alpha) and
                       "FromPma" (converts input PMA images to non-premultiplied alpha). Non case-sensitive.

    -p, --pngonly      (Default: True) Changes only files with '.png' extension.

    -r, --recursive    (Default: True) When given directories, will look down the entire directory tree.

    -v, --verbose      (Default: False) Prints all messages to standard output.

    --help             Displays help screen.


### NuGet references
You may notice that NuGet packages are not in the repository, so do not forget to set up package restoration in Visual Studio:

Tools menu → Options → Package Manager → General → "Allow NuGet to download missing packages during build" should be selected. 

If you have a build server then it needs to be setup with an environment variable 'EnableNuGetPackageRestore' set to true.
