using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    /// <summary>
    /// Модель сцены в квесте
    /// </summary>
    public class Stage
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название сцены
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Изображение обложки
        /// </summary>
        public int? CoverId { get; set; }
        public virtual Image Cover { get; set; }

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

        [InverseProperty("NextStage")]
        public virtual ICollection<Motion> MotionComeFrom { get; set; }
        
        public Stage()
        {
            Motions = new List<Motion>();
            MotionComeFrom = new List<Motion>();
        }
    }
}
