using TransitiveDependency;

namespace DirectDependency
{
    public class LogicWrapperProcessor
    {
        private readonly ISomeLogicProcessor _someLogicProcessor = new SomeLogicProcessor();

        // Method that will CRASH in diamond scenario (uses breaking namespace change)
        public string Compute(string input)
        {
            input += " - hello from direct dependency A";
            ICalculationResult explicitlyTypedResult = _someLogicProcessor.Calculate(input);
            return explicitlyTypedResult.Result;
        }

        // Method that will WORK even in diamond scenario (no namespace dependency)
        public string ComputeSimple(string input)
        {
            input += " - hello from direct dependency A (simple)";
            return _someLogicProcessor.CalculateSimple(input);
        }
    }
}