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
            var rnd = new Random();

            for (var i = 1; i < 5; i++)
            {
                var quest = new Quest()
                {
                    Title = "Квест номер - " + i,
                    Rate = rnd.Next(10),
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
                        Points = rnd.Next(100),
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
            }

            var result = dbContext.SaveChanges();
        }
    }
}
