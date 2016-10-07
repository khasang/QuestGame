using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Models
{
    public class UserQuestsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Rate { get; set; }
        public bool Active { get; set; }
    }
}