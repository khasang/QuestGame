using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using QuestGame.Domain;
using QuestGame.Domain.Entities;
using Newtonsoft.Json;

namespace QuestGame.WebApi.Controllers
{
    public class QuestsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Quests
        public IEnumerable<Quest> GetQuests()
        {
            var t = db.Quests;
            return JsonConvert.SerializeObject(t);

        }

        // GET: api/Quests/5
        [ResponseType(typeof(Quest))]
        public async Task<IHttpActionResult> GetQuest(int id)
        {
            Quest quest = await db.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            return Ok(quest);
        }

        // PUT: api/Quests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuest(int id, Quest quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quest.Id)
            {
                return BadRequest();
            }

            db.Entry(quest).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Quests
        [ResponseType(typeof(Quest))]
        public async Task<IHttpActionResult> PostQuest(Quest quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quests.Add(quest);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = quest.Id }, quest);
        }

        // DELETE: api/Quests/5
        [ResponseType(typeof(Quest))]
        public async Task<IHttpActionResult> DeleteQuest(int id)
        {
            Quest quest = await db.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }

            db.Quests.Remove(quest);
            await db.SaveChangesAsync();

            return Ok(quest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestExists(int id)
        {
            return db.Quests.Count(e => e.Id == id) > 0;
        }
    }
}