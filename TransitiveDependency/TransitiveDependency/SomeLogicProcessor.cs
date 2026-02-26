namespace TransitiveDependency
{
    public class Processor : IProcessor
    {
        public string Process(string input)
        {
            return input + " - TransitiveDependency v2.0.0";
        }
    }
}