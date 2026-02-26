namespace MyLibrary
{
    public class Processor : IProcessor
    {
        public string Process(string input)
        {
            return input + " - MyLibrary v2.0.0";
        }
    }
}