using TransitiveDependency;

namespace DirectDependency;

public class LogicWrapperProcessorB
{
    private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

    public string Compute(string input)
    {
        input += " - Hello from Direct dependency B v2";
        return _someLogicProcessor.Calculate(input).Result;
    }
}