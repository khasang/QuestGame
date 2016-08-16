using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class Motion
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Следующая сцена
        /// </summary>
        public int? NextStageId { get; set; }
        public virtual Stage NextStage { get; set; }

        /// <summary>
        /// Внешний ключ, указывающий на сцену
        /// </summary>
        public int OwnerStageId { get; set; }
        public virtual Stage OwnerStage { get; set; }

        //public Motion()
        //{
        //    Description = string.Empty;
        //}
    }
}
