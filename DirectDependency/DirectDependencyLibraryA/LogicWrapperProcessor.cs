using TransitiveDependency;

namespace DirectDependency;

public class LogicWrapperProcessor
{
    private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

    public string Compute(string input)
    {
        var result = _someLogicProcessor.Calculate(input).Result;

        var expectedBehavior = $"{input} - {"Transitive dependency v1"}";
        
        if (result != expectedBehavior)
        {
            throw new Exception($"Expected behavior {expectedBehavior}, but was {result}");
        }
        
        return result;
    }
}