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
    public class OperationsVM_n
    {
        public OperationsVM_n(int? owner)
        {
            this.StageId = (int)owner;
            this.StagesList = FillStagesListFull().Result;
        }

        [Required(ErrorMessage = "Описание не может быть пустым")]
        [Display(Name = "Описание действия")]
        public string Description { get; set; }

        public int StageId { get; set; }

        public Dictionary<int, string> StagesList { get; set; }

        private async Task<Dictionary<int, string>> FillStagesList()
        {
            var resultList = new Dictionary<int, string>();

            var quest = getQuests(getStages(StageId).Result).Result;

            foreach (var item in quest.Stages)
            {
                resultList.Add(item.Id, item.Title);
            }

            return resultList;
        }


        private async Task<Dictionary<int, string>> FillStagesListFull()
        {
            var resultList = new Dictionary<int, string>();

            IRequest clientStage = new DirectRequest();
            var requestStage = await clientStage.GetRequestAsync(@"api/Stages");
            var responseStage = await requestStage.Content.ReadAsAsync<IEnumerable<StageDTO>>();

            var stage = responseStage.FirstOrDefault(s => s.Id == StageId);

            IRequest clientQuest = new DirectRequest();
            var requestQuest = await clientQuest.GetRequestAsync(@"api/Quests");
            var responseQuest = await requestQuest.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

            var quest = responseQuest.FirstOrDefault(q => q.Id == stage.QuestId);

            foreach (var item in quest.Stages)
            {
                resultList.Add(item.Id, item.Title);
            }

            return resultList;
        }


        private async Task<StageDTO> getStages(int owner)
        {
            IRequest clientStage = new DirectRequest();
            var requestStage = await clientStage.GetRequestAsync(@"api/Stages");
            var responseStage = await requestStage.Content.ReadAsAsync<IEnumerable<StageDTO>>();

            var stage = responseStage.FirstOrDefault(s => s.Id == StageId);
            return stage;
        }

        private async Task<QuestDTO> getQuests(StageDTO stage)
        {
            IRequest clientQuest = new DirectRequest();
            var requestQuest = await clientQuest.GetRequestAsync(@"api/Quests");
            var responseQuest = await requestQuest.Content.ReadAsAsync<IEnumerable<QuestDTO>>();

            var quest = responseQuest.FirstOrDefault(q => q.Id == stage.QuestId);

            return quest;
        }
    }
}