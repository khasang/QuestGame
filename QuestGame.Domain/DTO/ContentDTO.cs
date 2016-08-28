using System;

namespace QuestGame.Domain.DTO
{
    public class ContentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}