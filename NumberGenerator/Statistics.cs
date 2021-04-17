namespace NumberGenerator
{
    public class Statistics
    {
        public long Calls { get; private set; }

        public void Reset()
        {
            Calls = 0;
        }

        public void Increment()
        {
            Calls++;
        }
    }
}
