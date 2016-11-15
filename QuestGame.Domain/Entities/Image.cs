using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Constants;

namespace QuestGame.Domain.Entities
{
    /// <summary>
    /// Модель файла изображения
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Идентификатор изображения
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя файла изображения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Относительный раздел хранения
        /// </summary>
        public string Prefix { get; set; }
    }
}
