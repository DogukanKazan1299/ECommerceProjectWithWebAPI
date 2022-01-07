using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Responses
{
    public class SuccessApiDataResponse<T>:ApiDataResponse<T>
    {
        public SuccessApiDataResponse(T data):base(true)
        {
            Data = data;
        }
        public SuccessApiDataResponse(T data,string message) : base(true,message)
        {
            Data = data;
        }
    }
}
