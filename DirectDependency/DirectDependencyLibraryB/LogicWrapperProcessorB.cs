using TransitiveDependency;

namespace DirectDependency
{
    public class WorkerB
    {
        private readonly IProcessor _processor = new Processor();

        public string DoWorkB(string input)
        {
            return "DirectDependencyB: " + _processor.Process(input);
        }
    }
}