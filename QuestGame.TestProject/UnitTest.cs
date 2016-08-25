using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuestGame.Domain.Interfaces;
using QuestGame.Common.Interfaces;
using AutoMapper;
using QuestGame.WebApi.Controllers;
using QuestGame.Domain.Entities;
using System.Collections.Generic;
using QuestGame.Domain.DTO;

namespace QuestGame.TestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mapperMock = new Mock<IMapper>().Object;
            var loggerMock = new Mock<ILoggerService>().Object;

            var questRepositoryMock = new Mock<IQuestRepository>()
            .Setup(x => x.GetAll())
            .Returns(new List<Quest>
            {
                new Quest{ Title = "1" },
                new Quest{ Title = "2" },
                new Quest{ Title = "3" }
            });

            var dataManagerMoq = new Mock<IDataManager>()
                                    .Setup(x => x.Quests)
                                    .Returns<IQuestRepository>(questRepositoryMock.Object);

            var controller = new QuestController(dataManagerMoq, mapperMock, loggerMock);
            var result = new List<QuestDTO>();
            result.AddRange(controller.GetQuest());

            Assert.AreEqual(result.Count, 3);
        }

        public IQuestRepository GetQuestRepository()
        {
            return new QuestRepositoryFake();
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
