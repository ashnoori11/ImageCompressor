Console.WriteLine("  hello and welcome to image compressor");
Console.WriteLine("  we need three things : ");
Console.WriteLine("  1- current folder path ");
Console.WriteLine("  2- your target folder path ");
Console.WriteLine("  3- pick a number from 30 to 100 for quality of your images");

using Compressor cmp = new();
string? doContinue = string.Empty;
List<string> withErrors = new();

string? currentPath = string.Empty;
string? targetPath = null;
int width = 0;
int height = 0;
int quality = 30;
int num = 0;

while (doContinue != "no")
{
    withErrors.Clear();
    doContinue = string.Empty;
    currentPath = string.Empty;
    targetPath = null;
    width = 0;
    height = 0;
    quality = 30;

    Console.WriteLine("  pls enter the path of your folder (the directory which contains your images) : ");
    currentPath = Console.ReadLine();

    Console.WriteLine("  pls enter the path of where you want to save your images after process (If you don't have a folder for this purpose, don't worry and press 'enter') : ");
    targetPath = Console.ReadLine();

    Console.WriteLine("  pls enter a number from 10 to 100 . This number represents the quality you want your image to have (The image quality starts from 100, which means high quality, and continues to 10, which means the lowest quality) : ");
    int.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Number, null, out quality);

    Console.WriteLine("  pls enter the width of your image :");
    int.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Number, null, out width);

    Console.WriteLine("  pls enter the width of your image :");
    int.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Number, null, out height);

    if (string.IsNullOrWhiteSpace(currentPath))
    {
        Console.WriteLine($"{nameof(currentPath)} can not be null or empty");
        continue;
    }

    DirectoryInfo dir = new DirectoryInfo(currentPath);
    FileInfo[] imageFiles = dir.GetFiles("*.jpg");

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Found {0} *.jpg files\n", imageFiles.Length);

    Console.ForegroundColor = ConsoleColor.Yellow;
    num = 0;
    while (num < imageFiles.Length)
    {
        try
        {
            Console.WriteLine("/---------------------------------------------------/");
            Console.WriteLine("  File name: {0}", imageFiles[num].Name);
            Console.WriteLine("  File size: {0}", imageFiles[num].Length);
            Console.WriteLine("  Creation: {0}", imageFiles[num].CreationTime);
            Console.WriteLine("  Attributes: {0}", imageFiles[num].Attributes);

            cmp.ProccessImage(options =>
            {
                options.Path = imageFiles[num].DirectoryName ?? string.Empty;
                options.ImageName = imageFiles[num].Name;
                options.Width = width;
                options.Height = height;
                options.Quality = quality;
                options.NewPath = targetPath ?? string.Empty;
            });

            Console.WriteLine("/---------------------------------------------------/");
            Console.WriteLine(" ");
        }
        catch (Exception exp)
        {
            withErrors.Add($"{imageFiles[num].Name} - Error Message : {exp.Message}");
            num++;
            continue;
        }

        num++;
    }

    num = 0;

    if (withErrors.Count() > 0)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.White;
        foreach (var item in withErrors)
        {
            Console.WriteLine(item);
        }
    }

    Console.WriteLine("************************************* done");
    Console.WriteLine("");

    Console.WriteLine("  do you want to continue ? (for exit and close type : 'no' otherwise type : 'y')");
    doContinue = Console.ReadLine();
}

Console.ForegroundColor = ConsoleColor.Red;

Console.WriteLine("  Thank you for choosing me to do the job");
Console.WriteLine("  Hope to meet");
Console.ReadKey();