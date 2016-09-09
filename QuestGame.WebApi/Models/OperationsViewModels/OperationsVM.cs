using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using QuestGame.Domain.Interfaces;
using QuestGame.Domain.Implementations;
using System.Threading.Tasks;
using QuestGame.Domain.DTO;

namespace QuestGame.WebApi.Models
{
    public class OperationsVM
    {
        public OperationsVM()
        {
            this.StagesList = new Dictionary<int, string>();
        }

        [Required(ErrorMessage = "Описание не может быть пустым")]
        [Display(Name = "Описание действия")]
        public string Description { get; set; }

        public int RedirectToId { get; set; }

        public int StageId { get; set; }

        public Dictionary<int, string> StagesList { get; set; }

    }
}