using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class Stage
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название сцены
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текстовое тело сцены
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Стоимость сцены
        /// </summary>
        public int Point { get; set; }
        
        /// <summary>
        /// Внешний ключ, указывающий на квест
        /// </summary>
        public int QuestId { get; set; }
        [JsonIgnore]
        public virtual Quest Quest { get; set; }

        /// <summary>
        /// Коллекция действий
        /// </summary>
        [InverseProperty("OwnerStage")]
        public virtual ICollection<Motion> Motions { get; set; }
        
        public Stage()
        {
            Motions = new List<Motion>();
        }
    }
}
