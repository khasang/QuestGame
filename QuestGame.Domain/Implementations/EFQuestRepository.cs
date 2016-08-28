using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;

namespace QuestGame.Domain.Implementations
{
    public class EFQuestRepository : IQuestRepository
    {
        private IApplicationDbContext dbContext;

        public EFQuestRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Quest> GetAll()
        {
            return dbContext.Quests;
        }

        public Quest GetById(object id)
        {
            return dbContext.Quests.Find((int)id);
        }

        public Quest GetByTitle(string title)
        {
            return dbContext.Quests.FirstOrDefault(x => x.Title == title);
        }

        public void Add(Quest item)
        {
            dbContext.Quests.Add(item);
        }

        public void Delete(object id)
        {
            Delete(GetById((int)id));
        }

        public void Delete(Quest item)
        {
            dbContext.Quests.Remove(item);
        }

        public void DeleteByTitle(string title)
        {
            var quest = dbContext.Quests.FirstOrDefault(x => x.Title == title);
            if (quest != null)
                Delete(quest);          
        }

        public void Update(Quest item)
        {
            dbContext.EntryObj(item);
        }
    }
}
