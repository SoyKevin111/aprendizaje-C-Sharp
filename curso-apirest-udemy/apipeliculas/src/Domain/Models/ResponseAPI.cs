using System.Net;

namespace apipeliculas.src.Domain.Models
{
    public class ResponseAPI
    {
        public ResponseAPI()
        {
            ErrorMessages = [];
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}