namespace Dtos
{
    public class CallStatisticDto
    {
        public CallStatisticDto(long durationInMilliseconds, long number, long[] result)
        {
            DurationInMilliseconds = durationInMilliseconds;
            Number = number;
            Result = result;
        }

        public long DurationInMilliseconds { get; }

        public long Number { get; }

        public long[] Result { get; }
    }
}