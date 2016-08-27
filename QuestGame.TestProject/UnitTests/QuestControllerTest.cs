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
        Mock<ILoggerService> loggerMock;
        Mock<IDataManager> dataManagerMoq;
        Mock<IMapper> mapperMock;

        [TestInitialize]
        public void TestInitialize()
        {
            loggerMock = new Mock<ILoggerService>();

            dataManagerMoq = new Mock<IDataManager>();
            dataManagerMoq.Setup(x => x.Quests.GetAll())
                          .Returns(new List<Quest>
                          {
                              new Quest {Title = "1"},
                              new Quest {Title = "2"},
                              new Quest {Title = "3"}
                          });
            dataManagerMoq.Setup(x => x.Quests.GetById(It.IsAny<int>()))
                          .Returns(new Quest { Title = "1", Id = 1 });

            mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(It.IsAny<IEnumerable<Quest>>()))
                      .Returns(dataManagerMoq.Object.Quests.GetAll().Select(x => new QuestDTO { Title = x.Title }));
            mapperMock.Setup(x => x.Map<Quest, QuestDTO>(It.IsAny<Quest>()))
                      .Returns(new QuestDTO { Title = dataManagerMoq.Object.Quests.GetById(It.IsAny<int>()).Title });

        }

        [TestMethod]
        public void Quest_GetAll_Count3()
        {
            // arrange
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
        public void Quest_GetById_Title1()
        {
            // arrange
            var controller = new QuestController(dataManagerMoq.Object, mapperMock.Object, loggerMock.Object);

            // act
            var result = controller.GetById(1);

            // assert
            Assert.AreEqual(result.Title, "1");
            Assert.IsInstanceOfType(result, typeof(QuestDTO));
        }
    }

}
