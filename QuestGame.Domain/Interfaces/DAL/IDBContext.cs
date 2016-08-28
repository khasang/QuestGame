using Microsoft.AspNet.Identity.EntityFramework;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Interfaces
{
    public interface IDBContext : IDisposable
    {
        DbSet<Quest> Quests { get; set; }
        DbSet<Stage> Stages { get; set; }
        DbSet<Operation> Operations { get; set; }

        DbSet<ContentQuest> QuestContents { get; set; }
        DbSet<ContentStage> StageContents { get; set; }
    }
}
