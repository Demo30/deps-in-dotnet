using TransitiveDependency;

namespace DirectDependency
{
    public class LogicWrapperProcessor
    {
        private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

        public string Compute(string input)
        {
            return _someLogicProcessor.Calculate(input + " - via DirectDependencyA");
        }
    }
}
