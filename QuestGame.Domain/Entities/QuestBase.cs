using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuestGame.Domain;
using System.Collections;

namespace QuestGame.Domain
{
    public abstract class QuestBase
    {
        protected DateTime modifyDate;
        protected ICollection<Stage> stages;

        public int Id { get; set; }
        public bool Active { get; set; }
//        public Entities.ApplicationUser User { get; set; }
        public string Title { get; set; }
        public int CountComplite { get; set; }
        public int Rate { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifyDate { get { return this.modifyDate; } set { this.modifyDate = DateTime.Now; } }


        /// <summary>
        /// Ссылка на содержимое для Квеста. Связь в БД 1-1
        /// </summary>
        public QuestContent QuestContent { get; set; }

        /// <summary>
        /// Коллекция Сцен для Квеста. Связь 1-∞
        /// </summary>
        public virtual ICollection<Stage> Stages { get; set; }

        public QuestBase()
        {
            this.stages = new List<Stage>();
        }

    }
}