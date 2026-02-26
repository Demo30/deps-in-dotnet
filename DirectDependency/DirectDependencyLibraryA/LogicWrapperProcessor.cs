using MyLibrary;

namespace DirectLibrary
{
    public class Worker
    {
        private readonly IProcessor _processor = new Processor();

        public string DoWork(string input)
        {
            return "DirectLibrary: " + _processor.Process(input);
        }
    }
}