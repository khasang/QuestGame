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
using QuestGame.Domain.DTO;

namespace QuestGame.TestProject.UnitTests
{
    [TestClass]
    public class QuestControllerTest
    {
        [TestMethod]
        public void Quest_GetAll_Count3()
        {
            // arrange
            var loggerMock = new Mock<ILoggerService>();

            var questRepositoryMock = new Mock<IQuestRepository>();
            questRepositoryMock.Setup(x => x.GetAll())
                .Returns(new List<Quest>
                {
                    new Quest {Title = "1"},
                    new Quest {Title = "2"},
                    new Quest {Title = "3"}
                });

            var dataManagerMoq = new Mock<IDataManager>();
            dataManagerMoq.Setup(x => x.Quests)
                .Returns(questRepositoryMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(It.IsAny<IEnumerable<Quest>>()))
                .Returns(dataManagerMoq.Object.Quests.GetAll().Select(x => new QuestDTO {Title = x.Title}));

            var controller = new QuestController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            var result = controller.GetQuest();

            // assert
            Assert.AreEqual(result.Count(), 3);
            Assert.IsInstanceOfType(result, typeof (IEnumerable<QuestDTO>));
            Assert.AreEqual(result.ElementAt(0).Title, "1");
            Assert.AreEqual(result.ElementAt(1).Title, "2");
            Assert.AreEqual(result.ElementAt(2).Title, "3");
        }

        [TestMethod]
        public void Quest_GetById_Count3()
        {
            var loggerMock = new Mock<ILoggerService>();

            var questRepositoryMock = new Mock<IQuestRepository>();
            questRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Quest { Title = "1", Id = 1 });

            var dataManagerMoq = new Mock<IDataManager>();
            dataManagerMoq.Setup(x => x.Quests)
                .Returns(questRepositoryMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<Quest, QuestDTO>(It.IsAny<Quest>()))
                .Returns(new QuestDTO { Title = dataManagerMoq.Object.Quests.GetById(1).Title });

            var controller = new QuestController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            var result = controller.GetById(1);

            // assert
            Assert.AreEqual(result.Title, "1");
        }
    }

    public class QuestRepositoryFake : IQuestRepository
    {
        public IEnumerable<Quest> GetAll()
        {
            return new List<Quest>
            {
                new Quest{ Title = "1" },
                new Quest{ Title = "2" },
                new Quest{ Title = "3" }
            };
        }

        public Quest GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Add(Quest item)
        {
            throw new NotImplementedException();
        }

        public void Update(Quest item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Quest item)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }
    }
}
