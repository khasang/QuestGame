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

            for (int i = 0; i < 3; i++) // Quest
            {
                var quest = new Quest
                {
                    Title = "Title" + i,
                    Date = DateTime.Now,
                    Active = true,
                    Rate = rnd.Next(10),
                    Owner = owner,
                    Stages = new List<Stage>
                    {
                        new Stage { Title = "Stage1", Body = "Body1", Point = rnd.Next(5) },
                        new Stage { Title = "Stage2", Body = "Body2", Point = rnd.Next(5) },
                        new Stage { Title = "Stage3", Body = "Body3", Point = rnd.Next(5) }
                    }
                };

                foreach (var item in quest.Stages)
                {
                    item.Motions = new List<Motion>
                    {
                        new Motion { Description = "Description" + rnd.Next(100).ToString() },
                        new Motion { Description = "Description" + rnd.Next(100).ToString() },
                        new Motion { Description = "Description" + rnd.Next(100).ToString() }
                    };
                }

                dbContext.Quests.Add(quest);
            };

            dbContext.SaveChanges();
        }
    }
}
