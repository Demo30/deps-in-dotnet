using TransitiveDependency;

namespace DirectDependency;

public class LogicWrapperProcessor
{
    private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

    public string Compute(string input)
    {
        return _someLogicProcessor.Calculate(input).Result;
    }
    
    public string Compute2(string input)
    {
        return _someLogicProcessor.Calculate2(input).Result;
    }
}