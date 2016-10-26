using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Implementations
{
    public class EFImageRepository : IImageRepository
    {
        private IApplicationDbContext dbContext;

        public EFImageRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Image> GetAll()
        {
            return dbContext.Images;
        }

        public Image GetById(object id)
        {
            return dbContext.Images.Find((int)id);
        }

        public void Add(Image item)
        {
            dbContext.Images.Add(item);
        }

        public void Update(Image item)
        {
            dbContext.EntryObj(item);
        }

        public void Delete(Image item)
        {
            dbContext.Images.Remove(item);
        }

        public void Delete(object id)
        {
            Delete(GetById((int)id));
        }
    }
}
