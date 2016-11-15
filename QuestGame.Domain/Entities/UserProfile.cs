using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.Entities
{
    /// <summary>
    /// Модель профайла пользователя
    /// </summary>
    public class UserProfile
    {
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Изображение аватарки
        /// </summary>
        public int? AvatarId { get; set; }
        public virtual Image Avatar { get; set; }


        /// <summary>
        /// Игровой рейтинг
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Кол-во пройденных квестов
        /// </summary>
        public int CountCompliteQuests { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime? InviteDate { get; set; }
    }
}
