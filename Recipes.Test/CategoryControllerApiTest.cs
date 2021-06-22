using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Recipes.Controllers;
using Recipes.Domain;
using Recipes.Features.Categories;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using List = Recipes.Features.Categories.List;

namespace Recipes.Test
{
    public class CategoryControllerApiTest
    {
        private CategoryController controller;
        private Mock<IMediator> mediatorMock;

        public CategoryControllerApiTest()
        {
            mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void Get_ReturnsActionResult_WithCategoriesEnvelope()
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

        [Fact]
        public async void Get_ReturnsCategoriesEnvelope()
        {
            controller = new CategoryController(mediatorMock.Object);
            mediatorMock.Setup(m => m.Send(It.IsAny<List.Query>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List.CategoriesEnvelope(new List<Category>(), new int()))
               .Verifiable("CategoriesEnvelope was not sent");

            var response = (await controller.List());

            Assert.IsType<Features.Categories.List.CategoriesEnvelope>(response.Value);

        }

        [Fact]
        public async System.Threading.Tasks.Task Post_ReturnsCreatedCategory()
        {
            controller = new CategoryController(mediatorMock.Object);

            Create.Command command = new Create.Command()
            {
                Name = "Drinks"
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Category())
               .Verifiable("Category was not sent");

            var response = await controller.Create(command);

            Assert.IsType<Category>(response.Value);
        }

        [Fact]
        public async void Delete_ReturnsUnit()
        {
            controller = new CategoryController(mediatorMock.Object);

            Delete.Command command = new Delete.Command()
            {
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<Delete.Command>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(Unit.Value)
              .Verifiable("Category was not sent");

            var response = await controller.Delete(command);


            Assert.IsType<Unit>(response.Value);
        }
        //[Fact]
        //public void Post_ReturnsBadRequest_ForExistingCategory()
        //{
        //    controller = new CategoryController(mediatorMock.Object);

        //    mediatorMock.Setup(m => m.Send(It.IsAny<Create.Command>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new Unit());
        //}
    }
}
