using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Recipes.Controllers;
using Recipes.Features.Recipes;
using Xunit;

namespace Recipes.Test
{
    public class RecipesApiTest
    {
        private RecipeController controller;
        private Mock<IMediator> mediatorMock;

        public RecipesApiTest()
        {
            mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void Get_WhenCalled_ReturnsActionResult()
        {
            //Arrange
            controller = new RecipeController(mediatorMock.Object);
            mediatorMock.Setup(m => m.Send(It.IsAny<List.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List.RecipesEnvelope(new List<RecipeDto>(), new int()))
                .Verifiable("RecipesEnvelope was not sent");

            //Act
            var response = controller.List();

            //Assert
            Assert.IsType<ActionResult<List.RecipesEnvelope>>(response.Result);
        }

    }
}
