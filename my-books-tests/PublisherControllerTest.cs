using Microsoft.EntityFrameworkCore;
using my_books.Controllers;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Models.ViewModels;
using my_books.Data.Services;
using my_books.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace my_books_tests
{
    public class PublisherControllerTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbControllerTest")
            .Options;

        AppDbContext context;
        PublishersService publishersService;
        PublishersController publishersController;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(context);

            publishersController = new PublishersController(publishersService, new NullLogger<PublishersController>());
        }

        [Test, Order(1)]
        public void HTTPGET_GetAllPublishers_WithSortSearchPageNr_ReturnOk_Test()
        {
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc", "Publisher", 1);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionREsultData = (actionResult as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionREsultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionREsultData.First().Id, Is.EqualTo(6));
            Assert.That(actionREsultData.Count, Is.EqualTo(5));

            IActionResult actionResultSecondPage = publishersController.GetAllPublishers("name_desc", "Publisher", 2);
            Assert.That(actionResultSecondPage, Is.TypeOf<OkObjectResult>());
            var actionREsultDataASecondPage = (actionResultSecondPage as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionREsultDataASecondPage.First().Name, Is.EqualTo("Publisher 1"));
            Assert.That(actionREsultDataASecondPage.First().Id, Is.EqualTo(1));
            Assert.That(actionREsultDataASecondPage.Count, Is.EqualTo(1));
        }

        [Test, Order(2)]
        public void HTTPGET_GetPublisherById_ReturnsOk_Test()
        {
            IActionResult actionResult = publishersController.GetPublisherById(1);

            Assert.NotNull(actionResult);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            Assert.That((actionResult as OkObjectResult).StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That((actionResult as OkObjectResult).Value, Is.TypeOf<Publisher>());
            Assert.That(((actionResult as OkObjectResult).Value as Publisher).Name, Is.EqualTo("Publisher 1").IgnoreCase);
        }

        [Test, Order(3)]
        public void HTTPGET_GetPublisherById_ReturnsNotFound_Test()
        {
            IActionResult actionResult = publishersController.GetPublisherById(99);
            //Assert.That(actionResult, Is.InstanceOf<NotFoundResult>());
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
            //Assert.That((actionResult as NotFoundResult).StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test, Order(4)]
        public void HTTPPOST_AddPublisher_ReturnsCreated_Test()
        {
            var newPublisherVM = new PublisherVM()
            {
                Name = "New Publisher"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<CreatedResult>());
        }

        [Test, Order(5)]
        public void HTTPPOST_AddPublisher_ReturnsbadRequest_Test()
        {
            var newPublisherVM = new PublisherVM()
            {
                Name = "123 New Publisher"
            };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(6)]
        public void HTTPDELETE_DeletePublisherById_RetunsOk_Test()
        {
            int publisherId = 6;
            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);

            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public void HTTPDELETE_DeletePublisherById_ReturnsBadRequest_Test()
        {
            int publisherId = 6;
            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {
            var publishers = new List<Publisher>
            {
                new Publisher()
                {
                    Id = 1,
                    Name = "Publisher 1"
                },
                new Publisher()
                {
                    Id = 2,
                    Name = "Publisher 2"
                },
                new Publisher()
                {
                    Id = 3,
                    Name = "Publisher 3"
                },
                 new Publisher()
                {
                    Id = 4,
                    Name = "Publisher 4"
                },
                new Publisher()
                {
                    Id = 5,
                    Name = "Publisher 5"
                },
                new Publisher()
                {
                    Id = 6,
                    Name = "Publisher 6"
                }
            };
            context.Publishers.AddRange(publishers);

            context.SaveChanges();

        }
    }
}
