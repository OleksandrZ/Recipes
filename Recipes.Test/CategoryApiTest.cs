using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Recipes.Controllers;
using Recipes.Domain;
using Recipes.Features.Recipes;
using Xunit;
using List = Recipes.Features.Categories.List;

namespace Recipes.Test
{
    public class CategoryApiTest
    {
        private CategoryController controller;
        private Mock<IMediator> mediatorMock;

        public CategoryApiTest()
        {
            mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void Get_WhenCalled_ReturnsActionResult()
        {
            controller = new CategoryController(mediatorMock.Object);
            mediatorMock.Setup(m => m.Send(It.IsAny<List.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List.CategoriesEnvelope(new List<Category>(), new int()))
                .Verifiable("CategoriesEnvelope was not sent");

            //Act
            var response = controller.List();

            //Assert
            Assert.IsType<ActionResult<List.CategoriesEnvelope>>(response.Result);
        }
    }
}
