// using DirectDependency;

using System;

class Program
{
    static void Main(string[] args)
    {
        // var directDep = new LogicWrapperProcessor();
        var input = "Main app";
        // var result = directDep.Compute(input);
        var result = "local test";
        Console.WriteLine($"Using the direct dependency processor for input '{input}', result = {result}");
    }
}
