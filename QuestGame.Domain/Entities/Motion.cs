using Newtonsoft.Json;
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
        [JsonIgnore]
        public virtual Stage NextStage { get; set; }

        /// <summary>
        /// Внешний ключ, указывающий на сцену
        /// </summary>
        public int OwnerStageId { get; set; }
        [JsonIgnore]
        public virtual Stage OwnerStage { get; set; }
    }
}
