using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class Quest
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название квеста
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Url обложки
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// Дата создания квеста
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Рейтинг квеста
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Флаг доступности квеста
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Внешний ключ, указывающий на автора квеста
        /// </summary>
        public string OwnerId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser Owner { get; set; }

        /// <summary>
        /// Коллекция сцен квеста
        /// </summary>
        public virtual ICollection<Stage> Stages { get; set; }
        public Quest()
        {
            Stages = new List<Stage>();
        }
    }
}
