using System;

namespace QuestGame.Domain.Entities
{
    public abstract class Content
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public DateTime ModifyDate { get; set; }

        public Content()
        {
            this.ModifyDate = DateTime.Now;
        }
    }
}