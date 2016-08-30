using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuestGame.Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using QuestGame.Domain.Entities;
using QuestGame.Domain.DTO;
using AutoMapper;
using QuestGame.WebApi.Controllers;
using System.Linq;

namespace QuestControllerTests
{
    [TestClass]
    public class QuestControllerTest
    {
        List<Quest> quests = new List<Quest>
        {
            new Quest { Title = "Первый пробный", Id = 1 },
            new Quest { Title = "Второй пробный", Id = 2 },
            new Quest { Title = "Третий пробный", Id = 3 }
        };


        [TestMethod]
        public void QuestController_GetQuests_Count_Three_Quests()
        {            
            // Arrange
            var dataManagerMock = new Mock<IDataManager>();            
            dataManagerMock.Setup(mtd => mtd.Quests.GetAll())
                .Returns(quests);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mp => mp.Map<IEnumerable<Quest>, IEnumerable<QuestDTO>>(It.IsAny<IEnumerable<Quest>>()))
                .Returns( quests.Select( it => new QuestDTO
                {
                    Id = it.Id,
                    Title = it.Title
                }));

            var controllerTest = new QuestsController( dataManagerMock.Object, mapperMock.Object );

            // Act
            var result = controllerTest.GetQuests().ToList();

            // Assert
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0].Id, 1);
            Assert.AreEqual(result[1].Id, 2);
            Assert.AreEqual(result[2].Id, 3);
        }
    }
}
