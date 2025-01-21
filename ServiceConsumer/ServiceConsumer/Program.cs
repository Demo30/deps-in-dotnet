// See https://aka.ms/new-console-template for more information

using DirectDependency;
using TransitiveDependency;

var directDepA = new LogicWrapperProcessor();
var directDepB = new LogicWrapperProcessorB();

string[] inputs = ["anotherPath", "mainApp", "test", "test_A"];

foreach (var input in inputs)
{
    string resultA;
    if (input == "anotherPath")
    {
        resultA = directDepA.Compute2(input);

    }
    else
    {
        resultA = directDepA.Compute(input);
        
    }
    
    Console.WriteLine($"Using the direct dependency A processor for input '{input}', result = {resultA}");
    
    var resultB = directDepB.Compute(input);
    Console.WriteLine($"Using the direct dependency B processor for input '{input}', result = {resultB}");
}

