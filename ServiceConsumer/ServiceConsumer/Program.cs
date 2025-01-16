// See https://aka.ms/new-console-template for more information

using DirectDependency;
using TransitiveDependency;

var directDepA = new LogicWrapperProcessor();
var input = "Main app";
var result = directDepA.Compute(input);

// IDE advises me to remove this check since result can never be null according to the types.

// if (result == null)
// {
//     throw new Exception("Oh no the result is null.");
// }
// else
// {
//     Console.WriteLine($"The result is {result.Length} chars long");    
// }

Console.WriteLine($"The result is {result.Length} chars long");

// Direct B
DifferentFunctionality.Hello();