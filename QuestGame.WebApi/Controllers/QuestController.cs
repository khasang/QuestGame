using Newtonsoft.Json;
using QuestGame.Domain;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuestGame.WebApi.Controllers
{
    [RoutePrefix("api/Quest")]
    public class QuestController : ApiController
    {
        DataManager dataManager;

        public QuestController()
        {
            this.dataManager = new DataManager();
        }

        [Route("Get")]
        public List<QuestDTO> GetQuest()
        {
            try
            {
                var quests = dataManager.Quests.GetAll().Select(x => new QuestDTO
                {
                    Title = x.Title,
                    Active = x.Active,
                    Date = x.Date,
                    Rate = x.Rate
                }).ToList();
                return quests;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }            
        }
    }

    public class QuestDTO
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Rate { get; set; }
        public bool Active { get; set; }
        public string OwnerName { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
        public QuestDTO()
        {
            //Title = "Quest";
            //Date = DateTime.Now;
            Stages = new List<Stage>();
        }
    }
}
