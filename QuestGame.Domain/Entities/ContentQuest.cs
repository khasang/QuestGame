using Newtonsoft.Json;
using System;

namespace QuestGame.Domain.Entities
{
    public class ContentQuest : Content
    {
        [JsonIgnore]
        public virtual Quest Quest { get; set; }
    }
}