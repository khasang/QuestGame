using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Interfaces
{
    public interface IRequest
    {
        Task<HttpResponseMessage> GetRequestAsync(string requestUri);
    }
}
