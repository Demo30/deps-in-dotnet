using TransitiveDependency;

namespace DirectDependencyB
{
    public class LogicWrapperProcessorB
    {
        private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

        public string Compute(string input)
        {
            return _someLogicProcessor.Calculate(input + " - via DirectDependencyB");
        }
    }
}
