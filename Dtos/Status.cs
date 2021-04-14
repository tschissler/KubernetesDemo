namespace Dtos
{
    public class StatusDto
    {
        public StatusDto(string status)
        {
            Status = status;
        }
        
        public string Status { get; }
    }
}