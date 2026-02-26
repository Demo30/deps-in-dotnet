using TransitiveDependency.NewNamespace;

namespace TransitiveDependency
{
    public class SomeLogicProcessor : ISomeLogicProcessor
    {
        public ICalculationResult Calculate(string input)
        {
            return new CalculationResult
            {
                Result = input + " - Transitive dependency v2.0.0"
            };
        }

        public string CalculateSimple(string input)
        {
            return input + " - Simple calculation v2.0.0";
        }
    }
}