namespace IMGRED
{
  public class CLIArgs
  {
    public string Option {get; set;} = "--help";
    public string InputFile {get; set;} = "input.bmp";
    public string OutputFile {get; set;} = "output.bmp";
  }

  public static class CLIParser
  {
    const string Pattern = "Usage:\n  imgred [<options>] [<input> | <output>]";

    public static CLIArgs Parse(string[] args)
    {
      if (args[0]=="--help")
        throw new ArgumentException($"{Pattern}");
      if (args.Length < 2)
        throw new ArgumentException($"Too few arguments.\n{Pattern}");
      else if (args.Length > 3)
        throw new ArgumentException($"Too many arguments.\n{Pattern}");

      CLIArgs result = new CLIArgs();

      result.Option = args[0];
      result.InputFile = args[1];

      if (args.Length == 3)
        result.OutputFile = args[2];


      return result;
    }
  }
}