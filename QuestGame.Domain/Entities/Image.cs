using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    public class Image
    {
        /// <summary>
        /// Идантификатор изображения
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Расширение изображения
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// Относительный путь хранения изображения
        /// </summary>
        public string Path { get; set; }

        public string GetPath()
        {
            return string.Format("{0}{1}.{2}", Path, Id, Ext);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
