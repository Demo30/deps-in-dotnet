
using TransitiveDependencyB;

namespace DirectDependency;

public class LogicWrapperProcessorB
{
    private readonly ISomeLogicProcessor _someLogicProcessor = new TransitiveDependencyBProcessor();

    public string Compute(string input)
    {
        return _someLogicProcessor.Calculate(input);
    }
}