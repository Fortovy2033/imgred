using IMGRED;

class Program
{
  static void Main(string[] args)
  {
    try
    {
      CLIArgs parsed = CLIParser.Parse(args);
      string option = parsed.Option;
      string inputFile = parsed.InputFile;
      string outputFile = parsed.OutputFile;

      Image image = new Image(inputFile);

      switch (option)
      {
        case "--help":
          Console.WriteLine("*helping*");
          break;

        case "--invert":
          Filters.Invert(image);
          break;

        case "--grayscale":
          Filters.Grayscale(image);
          break;

        case "--sepia":
          Filters.Sepia(image);
          break;

        case "--bright":
          Console.Write("Значение яркости (+/-): ");
          sbyte value = sbyte.Parse(Console.ReadLine());
          Filters.AdjustBrightness(image, value);
          break;

        case "--contrast":
          Filters.MakeContrast(image);
          break;

        case "--blur":
          Filters.BoxBlur(image);
          break;

        case "--channels":
          Console.Write("ΔR: ");
          sbyte dr = sbyte.Parse(Console.ReadLine());
          Console.Write("ΔG: ");
          sbyte dg = sbyte.Parse(Console.ReadLine());
          Console.Write("ΔB: ");
          sbyte db = sbyte.Parse(Console.ReadLine());
          Filters.AdjustChannels(image, dr, dg, db);
          break;

        default:
          throw new ArgumentException("No such option");
      }

      Console.WriteLine($"Сохранение результата в {outputFile}");
      image.Save(outputFile);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"imgred: {ex.Message}");
    }
  }
}