using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestGame.Domain
{
    public abstract class StageBase
    {
        protected DateTime modifyDate;
        protected ICollection<Operation> operations;

        public int Id { get; set; }
        public string Title { get; set; }
        public int Points { get; set; }
        public bool? AllowSkip { get; set; }
        public DateTime ModifyDate { get { return this.modifyDate; } set { this.modifyDate = DateTime.Now; } }

        public int OwnerId { get; set; }
        public Quest Owner { get; set; }

        /// <summary>
        /// Ссылка на содержимое для Сцены. Связь в БД 1-1
        /// </summary>
        public StageContent StageContent { get; set; }

        /// <summary>
        /// Коллекция действий для Сцены. Связь 1-∞
        /// </summary>
        public virtual ICollection<Operation> Operations { get; set; }

        public StageBase()
        {
            this.operations = new List<Operation>();
        }
    }
}