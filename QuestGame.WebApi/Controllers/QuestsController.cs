using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuestGame.Domain.Entities;


namespace QuestGame.WebApi.Controllers
{
    public class QuestsController : ApiController
    {
        QuestGame.Domain.ApplicationDbContext db = new QuestGame.Domain.ApplicationDbContext();

        // GET: api/Quests
        public IEnumerable<Quest> Get()
        {
                var t = db.Quests;
                return t;
        }

        // GET: api/Quests/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Quests
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Quests/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Quests/5
        public void Delete(int id)
        {
        }
    }
}
