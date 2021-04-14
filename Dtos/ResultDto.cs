using System.Collections.Generic;

namespace Dtos
{
    public class ResultDto
    {
        public ResultDto(IEnumerable<long> result)
        {
            Result = result;
        }

        public IEnumerable<long> Result { get; }
    }
}