using System;

namespace QuestGame.Domain.Entities
{
    public class ContentQuest : Content
    {
        public virtual Quest Quest { get; set; }
    }
}