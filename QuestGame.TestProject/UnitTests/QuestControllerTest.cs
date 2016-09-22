using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuestGame.Domain.Interfaces;
using QuestGame.Common.Interfaces;
using AutoMapper;
using QuestGame.WebApi.Controllers;
using QuestGame.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using QuestGame.Domain.DTO;

namespace QuestGame.TestProject.UnitTests
{
    [TestClass]
    public class QuestControllerTest
    {
        private const string USER = "admin@admin.com";
        private List<Quest> quests;
        private List<ApplicationUser> users;

        Mock<ILoggerService> loggerMock;
        Mock<IDataManager> dataManagerMoq;
        Mock<IMapper> mapperMock;        

        [TestInitialize]
        public void TestInitialize()
        {
            quests = new List<Quest>
            {
                GetQuest(1),
                GetQuest(2),
                GetQuest(3)
            };

            users = new List<ApplicationUser>
            {
                GetUser("admin@admin.com"),
                GetUser("user@user.com")
            };

            loggerMock = new Mock<ILoggerService>();
            loggerMock.Setup(x => x.Information(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()));

            // Настройка dataManager
            dataManagerMoq = new Mock<IDataManager>();

            dataManagerMoq.Setup(x => x.Quests.GetAll())
                          .Returns(quests);

            dataManagerMoq.Setup(x => x.Quests.GetById(It.IsAny<int>()))
                          .Returns<int>(y => quests.ElementAt(y - 1));

            dataManagerMoq.Setup(x => x.Quests.Add(It.IsAny<Quest>()))
                          .Callback<Quest>(y => quests.Add(y));

            dataManagerMoq.Setup(x => x.Quests.Delete(It.IsAny<Quest>()))
                          .Callback<Quest>(y => quests.Remove(y));

            dataManagerMoq.Setup(x => x.Quests.Update(It.IsAny<Quest>()))
                          .Callback<Quest>(y => UpdateQuest(y));

            dataManagerMoq.Setup(x => x.Users.GetAll())
                          .Returns(users);

            dataManagerMoq.Setup(x => x.Quests.DeleteByTitle(It.IsAny<string>()))
                          .Callback<string>(y =>
                          {
                              var q = quests.FirstOrDefault(z => z.Title == y);
                              quests.Remove(q);
                          });

            // Настройка Automapper
            mapperMock = new Mock<IMapper>();

            mapperMock.Setup(x => x.Map<QuestFullDTO, Quest>(It.IsAny<QuestFullDTO>()))
                      .Returns<QuestFullDTO>(y => QuestFullDtoToQuest(y));

            mapperMock.Setup(x => x.Map<IEnumerable<Quest>, IEnumerable<QuestFullDTO>>(It.IsAny<IEnumerable<Quest>>()))
                      .Returns(dataManagerMoq.Object.Quests.GetAll().Select(x => QuestToQuestFullDto(x)));

            mapperMock.Setup(x => x.Map<Quest, QuestFullDTO>(It.IsAny<Quest>()))
                      .Returns<Quest>(y => QuestToQuestFullDto(y));

            mapperMock.Setup(x => x.Map<QuestFullDTO, Quest>(It.IsAny<QuestFullDTO>(), It.IsAny<Quest>()))
                      .Callback<QuestFullDTO, Quest>((y, z) => QuestFullDtoToQuest(y, z));

        }

        [TestMethod]
        public void Quest_GetAll_Count3()
        {
            // arrange
            var controller = new QuestFullController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            var result = controller.GetAll();

            // assert
            Assert.AreEqual(result.Count(), 3);
            Assert.IsInstanceOfType(result, typeof (IEnumerable<QuestFullDTO>));
            Assert.AreEqual(result.ElementAt(0).Title, "TitleTest1");
            Assert.AreEqual(result.ElementAt(1).Title, "TitleTest2");
            Assert.AreEqual(result.ElementAt(2).Title, "TitleTest3");
        }

        [TestMethod]
        public void Quest_GetById_Title1()
        {
            // arrange
            var controller = new QuestFullController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            var result = controller.GetById(1);

            // assert
            Assert.AreEqual(result.Title, "TitleTest1");
            Assert.IsInstanceOfType(result, typeof(QuestFullDTO));
        }

        [TestMethod]
        public void Quest_Add_QuestValue()
        {
            // arrange
            var controller = new QuestFullController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            controller.Create(new QuestFullDTO
            {
                Id = 10,
                Title = "test",
                Owner = USER
            });

            // assert
            var result = quests.FirstOrDefault(x => x.Id == 10);
            Assert.AreEqual(result.Title, "test");
        }

        [TestMethod]
        public void Quest_Delete_NotQuestValue()
        {
            // arrange
            var controller = new QuestFullController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            controller.Delete(new QuestFullDTO
            {
                Id = 1,
                Title = "TitleTest1",
                Owner = USER
            });

            // assert
            var result = quests.FirstOrDefault(x => x.Id == 1);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Quest_Update_ChageQuestValue()
        {
            // arrange
            var controller = new QuestFullController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            controller.Update(new QuestFullDTO
            {
                Id = 2,
                Title = "TitleUpdate",
                Owner = USER
            });

            // assert
            var result = quests.FirstOrDefault(x => x.Id == 2);
            Assert.AreEqual(result.Title, "TitleUpdate");
        }


        private Quest GetQuest(int i)
        {
            var quest = new Quest
            {
                Id = i,
                Title = "TitleTest" + i,
                Date = DateTime.Now,
                Active = true,
                Rate = 50,
                Owner = GetUser(USER),
                Stages = new List<Stage>
                    {
                        new Stage { Title = "StageTest" + i, Body = "BodyTest" + i, Point = 5 },
                        new Stage { Title = "StageTest" + i, Body = "BodyTest" + i, Point = 6 },
                        new Stage { Title = "StageTest" + i, Body = "BodyTest" + i, Point = 7 }
                    }
            };

            foreach (var item in quest.Stages)
            {
                item.Motions = new List<Motion>
                    {
                        new Motion { Description = "DescriptionTest" + i },
                        new Motion { Description = "DescriptionTest" + i },
                        new Motion { Description = "DescriptionTest" + i }
                    };
            }

            return quest;
        }

        private ApplicationUser GetUser(string userName)
        {
            return new ApplicationUser
            {
                UserName = userName,
                Email = userName
            };
        }

        private void UpdateQuest(Quest item)
        {
            var quest = quests.FirstOrDefault(x => x.Id == item.Id);
            if (quest == null)
                throw new Exception();

            quest.Title = item.Title;
            quest.Date = item.Date;
            quest.Active = item.Active;
            quest.Rate = item.Rate;
            quest.Owner = item.Owner;
            quest.Stages = item.Stages;
        }

        private QuestFullDTO QuestToQuestFullDto(Quest item)
        {
            return new QuestFullDTO
            {
                Id = item.Id,
                Title = item.Title,
                Date = item.Date,
                Active = item.Active,
                Rate = item.Rate
            };
        }

        private Quest QuestFullDtoToQuest(QuestFullDTO item)
        {
            return new Quest
            {
                Id = item.Id,
                Title = item.Title,
                Date = item.Date,
                Active = item.Active,
                Rate = item.Rate,
                Owner = GetUser(USER)
            };
        }

        private void QuestFullDtoToQuest(QuestFullDTO item, Quest model)
        {
            model.Id = item.Id;
            model.Title = item.Title;
            model.Date = item.Date;
            model.Active = item.Active;
            model.Rate = item.Rate;
            model.Owner = GetUser(USER);
        }
    }

}
