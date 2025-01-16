using TransitiveDependency;

namespace DirectDependency;

public class LogicWrapperProcessorB
{
    private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

    public string Compute(string input)
    {
        var result = _someLogicProcessor.Calculate(input).Result;
        var expectedResult = $"{input} - Transitive dependency v1";

        if (result != expectedResult)
        {
            throw new Exception($"Expected {expectedResult} but was {result}");
        }
        
        return result;
    }
}