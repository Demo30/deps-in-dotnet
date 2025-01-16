// See https://aka.ms/new-console-template for more information

using DirectDependency;

var directDep = new LogicWrapperProcessor();
var input = "Main app";
var result = directDep.Compute(input);
Console.WriteLine($"Using the direct dependency processor for input '{input}', result = {result}");