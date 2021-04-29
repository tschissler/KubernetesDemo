namespace Dtos
{
    public class FullStatusDto
    {
        public FullStatusDto(bool status, long currentMaxValue, long currentInterval)
        {
            Status = status;
            CurrentMaxValue = currentMaxValue;
            CurrentInterval = currentInterval;
        }
        
        public bool Status { get; }

        public long CurrentMaxValue { get; set; }
        public long CurrentInterval { get; set; }
    }
}