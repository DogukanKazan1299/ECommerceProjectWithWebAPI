using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Responses
{
    public class ErrorApiDataResponse<T> : ApiDataResponse<T>
    {
        public ErrorApiDataResponse(T data):base(false)
        {
            Data = data;
        }
        public ErrorApiDataResponse(T data,string message):base(false,message)
        {
            Data = data;
        }
    }
}
