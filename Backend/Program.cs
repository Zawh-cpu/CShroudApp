namespace Backend;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var host = BackendStarter.Start(args, null);
        Console.WriteLine("Hello World");
    }
}