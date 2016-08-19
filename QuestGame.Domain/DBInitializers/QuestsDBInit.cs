using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestGame.Domain.Entities;

namespace QuestGame.Domain.DBInitializers
{
    class QuestsDBInit : IInitialization
    {
        public void Initialization(ApplicationDbContext dbContext)
        {
            for (var i = 1; i < 5; i++)
            {
                var quest = new Quest()
                {
                    Title = "Квест номер - " + i,
                    Rate = 5,
                    Content = new ContentQuest
                    {
                        Text = "Описание для Квеста - " + i
                    }
                };

                for (var s = 1; s < 4; s++)
                {
                    var stage = new Stage()
                    {
                        Title = "Сцена номер - " + s + " для Квеста № " + i,
                        Points = 50,
                        Content = new ContentStage()
                        {
                            Text = "Описание для Сцены - " + s
                        }
                    };


                    for (var o = 1; o < 3; o++)
                    {
                        var operation = new Operation
                        {
                            Description = "Действие № " + o + " для Сцены # " + s
                        };

                        stage.Operations.Add(operation);
                    }

                    quest.Stages.Add(stage);
                }

                dbContext.Quests.Add(quest);
            }           

            var result = dbContext.SaveChanges();
        }
    }
}
