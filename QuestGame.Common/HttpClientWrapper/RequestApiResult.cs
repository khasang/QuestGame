using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Common
{
    public class RequestApiResult
    {
        public RequestApiResult()
        {
            ResponseErrors = new List<string>();
        }

        public bool Succes { get; set; }
        public HttpStatusCode Status { get; set; }
        public dynamic ResponseData { get; set; }
        public ICollection<string> ResponseErrors { get; set; }
    }
}
