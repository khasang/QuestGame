using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuestGame.Domain.Entities
{
    public class ContentQuest : Content
    {
        [JsonIgnore]
        public virtual Quest Owner { get; set; }
    }
}