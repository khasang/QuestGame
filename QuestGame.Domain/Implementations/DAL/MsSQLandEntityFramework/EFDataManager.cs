using QuestGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Implementations
{
    public class EFDataManager : IDataManager
    {

        IQuestRepository questsRepository;
        IStageRepository stagesRepository;
        IOperationRepository operationsRepository;
        IContentQuestRepository contentQuestRepository;
        IContentStageRepository contentStageRepository;

        IDBContext db;

        public EFDataManager(IDBContext db)
        {
            this.db = db;
        }


        public IContentQuestRepository ContentQuest
        {
            get
            {
                if (this.contentQuestRepository == null)
                {
                    this.contentQuestRepository = new EFContentQuestRepository( db );
                }

                return contentQuestRepository;
            }
        }

        public IContentStageRepository ContentStage
        {
            get
            {
                if (this.contentStageRepository == null)
                {
                    this.contentStageRepository = new EFContentStageRepository( db );
                }

                return contentStageRepository;
            }
        }

        public IOperationRepository Operations
        {
            get
            {
                if (this.operationsRepository == null)
                {
                    this.operationsRepository = new EFOperationRepository( db );
                }

                return operationsRepository;
            }
        }

        public IQuestRepository Quests
        {
            get
            {
                if (this.questsRepository == null)
                {
                    this.questsRepository = new EFQuestRepository( this.db );
                }

                return questsRepository;
            }
        }

        public IStageRepository Stages
        {
            get
            {
                if (this.stagesRepository == null)
                {
                    this.stagesRepository = new EFStageRepository( db );
                }

                return stagesRepository;
            }
        }

        public void Save()
        {
           // db.SaveChanges();
        }


        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
