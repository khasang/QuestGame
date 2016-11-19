using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain.DBInitializers
{
    public class InitQuest : IInitialization
    {
        private static Random rnd = new Random();

        public void Initialization(ApplicationDbContext dbContext)
        {
            var owner = dbContext.Users.FirstOrDefault(x => x.Email == "admin@admin.com");

            for (var i = 0; i < 3; i++) // Quest
            {
                var stage1 = new Stage
                {
                    Title = "Stage1",
                    Body = "Body1",
                    Point = rnd.Next(5),
                    Cover = new Image
                    {
                        Name = @"http://localhost:9243/Content/Images/NoImageAvailable.png",
                        Prefix = string.Empty,
                    },
                };

                var quest = new Quest
                {
                    Title = "Title" + i,
                    Date = DateTime.Now,
                    Active = true,
                    Rate = rnd.Next(10),
                    Owner = owner,
                    Cover = new Image
                    {
                        Name = @"http://localhost:9243/Content/Images/NoImageAvailable.png",
                        Prefix = string.Empty,
                    },
                    Stages = new List<Stage>
                    {
                        stage1,
                        new Stage
                        {
                            Title = "Stage2",
                            Body = "Body2",
                            Point = rnd.Next(5),
                            Cover = new Image
                            {
                                Name = @"http://localhost:9243/Content/Images/NoImageAvailable.png",
                                Prefix = string.Empty,
                            },
                        },
                        new Stage
                        {
                            Title = "Stage3",
                            Body = "Body3",
                            Point = rnd.Next(5),
                            Cover = new Image
                            {
                                Name = @"http://localhost:9243/Content/Images/NoImageAvailable.png",
                                Prefix = string.Empty,
                            },
                        }
                    }
                };

                foreach (var item in quest.Stages)
                {
                    item.Motions = new List<Motion>
                    {
                        new Motion { Description = "Description" + rnd.Next(100).ToString() },
                        new Motion { Description = "Description" + rnd.Next(100).ToString() },
                        new Motion { Description = "Description" + rnd.Next(100).ToString(), NextStage = quest.Stages.ElementAt(0) }
                    };
                }

                dbContext.Quests.Add(quest);
            };

            dbContext.SaveChanges();
        }
    }
}
