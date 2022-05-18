using Jobsity.Chat.Api.Application.Chat;
using Jobsity.Chat.Api.MessageBroker.Abstractions.Producer;
using Jobsity.Chat.Api.Models;
using Jobsity.Chat.Api.Persistance;
using Jobsity.Chat.Api.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SignalR_UnitTestingSupportCommon.IHubContextSupport;

namespace Jobsity.Chat.Api.Aplication.Tests.Chat
{
    public class ChatProcessorTests
    {
        private Mock<IMessageProducer> _messageProducerMock;

        [SetUp]
        public void Setup()
        {
            _messageProducerMock = new Mock<IMessageProducer>();
            _messageProducerMock.Setup(x => x.SendMessage(It.IsAny<ChatMessage>()));
        }

        [Test]
        public async Task ProcessMessageAsync_CorrectMessage_NoCommand_Success()
        {
            // Arrange
            var iHubContextSupport = new UnitTestingSupportForIHubContext<ChatHub>();
            var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            var context = new ChatContext(options);

            var message = new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Jose",
                Message = "Message",
                MessageDateTime = DateTime.Now,
                Code = String.Empty,
                ChatRoom = String.Empty
            };

            var chatProcessor = new ChatProcessor(iHubContextSupport.IHubContextMock.Object, context, _messageProducerMock.Object);

            // Act
            await chatProcessor.ProcessMessageAsync(message);

            // Assert
            var expectedSender = message.MessageDateTime.ToString() + " " + "Jose";
            iHubContextSupport.ClientsAllMock
                .Verify(x => x.SendCoreAsync(
                    "ReceiveMessage",
                    new object[] { expectedSender, message.Message },
                    It.IsAny<CancellationToken>())
                );

            Assert.That(context.Messages.Count(), Is.EqualTo(1));
            Assert.That(context.Messages.First().Id, Is.EqualTo(message.Id));
            Assert.That(context.Messages.First().UserName, Is.EqualTo(message.UserName));
            Assert.That(context.Messages.First().Message, Is.EqualTo(message.Message));
            Assert.That(context.Messages.First().Code, Is.EqualTo(message.Code));
            Assert.That(context.Messages.First().ChatRoom, Is.EqualTo(message.ChatRoom));

            _messageProducerMock.Verify(x => x.SendMessage(It.IsAny<ChatMessage>()), Times.Never);
            Assert.Pass();
        }

        [Test]
        public async Task ProcessMessageAsync_CorrectMessage_Command_Success()
        {
            // Arrange
            var iHubContextSupport = new UnitTestingSupportForIHubContext<ChatHub>();
            var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;
            var context = new ChatContext(options);

            var message = new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Jose",
                Message = "/stock=aapl.us",
                MessageDateTime = DateTime.Now,
                Code = String.Empty,
                ChatRoom = String.Empty
            };

            var chatProcessor = new ChatProcessor(iHubContextSupport.IHubContextMock.Object, context, _messageProducerMock.Object);

            // Act
            await chatProcessor.ProcessMessageAsync(message);

            // Assert
            var expectedSender = message.MessageDateTime.ToString() + " " + "Jose";
            iHubContextSupport.ClientsAllMock
                .Verify(x => x.SendCoreAsync(
                    "ReceiveMessage",
                    new object[] { expectedSender, message.Message },
                    It.IsAny<CancellationToken>())
                );

            Assert.That(context.Messages.Count(), Is.EqualTo(1));
            Assert.That(context.Messages.First().Id, Is.EqualTo(message.Id));
            Assert.That(context.Messages.First().UserName, Is.EqualTo(message.UserName));
            Assert.That(context.Messages.First().Message, Is.EqualTo(message.Message));
            Assert.That(context.Messages.First().Code, Is.EqualTo("aapl.us"));
            Assert.That(context.Messages.First().ChatRoom, Is.EqualTo(message.ChatRoom));

            _messageProducerMock.Verify(x => x.SendMessage(It.IsAny<ChatMessage>()), Times.Once);
            Assert.Pass();
        }

        [Test]
        public void ProcessMessageAsync_NullMessage_Fail()
        {
            // Arrange
            var iHubContextSupport = new UnitTestingSupportForIHubContext<ChatHub>();
            var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;
            var context = new ChatContext(options);
            var chatProcessor = new ChatProcessor(iHubContextSupport.IHubContextMock.Object, context, _messageProducerMock.Object);

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(() => chatProcessor.ProcessMessageAsync(null));
        }


        [Test]
        public void ProcessMessageAsync_GetMessagesAsText_Empty()
        {
            // Arrange
            var iHubContextSupport = new UnitTestingSupportForIHubContext<ChatHub>();
            var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase3")
            .Options;
            var context = new ChatContext(options);
            var numberOfMessages = 2;

            var chatProcessor = new ChatProcessor(iHubContextSupport.IHubContextMock.Object, context, _messageProducerMock.Object);

            // Act
            var messages = chatProcessor.GetMessagesAsText(numberOfMessages);

            // Assert
            iHubContextSupport.ClientsAllMock
                .Verify(x => x.SendCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<object[]>(),
                    It.IsAny<CancellationToken>()), Times.Never);

            Assert.That(context.Messages, Is.Empty);

            _messageProducerMock.Verify(x => x.SendMessage(It.IsAny<ChatMessage>()), Times.Never);
            Assert.Pass();
        }

        [Test]
        public void ProcessMessageAsync_GetMessagesAsText_Sucess()
        {
            // Arrange
            var iHubContextSupport = new UnitTestingSupportForIHubContext<ChatHub>();
            var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase3")
            .Options;
            var context = new ChatContext(options);
            var message = new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Jose",
                Message = "Message",
                MessageDateTime = DateTime.Now,
                Code = String.Empty,
                ChatRoom = String.Empty
            };
            context.Add(message);
            context.SaveChanges();


            var numberOfMessages = 2;

            var chatProcessor = new ChatProcessor(iHubContextSupport.IHubContextMock.Object, context, _messageProducerMock.Object);

            // Act
            var messages = chatProcessor.GetMessagesAsText(numberOfMessages);

            // Assert
            iHubContextSupport.ClientsAllMock
                .Verify(x => x.SendCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<object[]>(),
                    It.IsAny<CancellationToken>()), Times.Never);

            var expectedValue = message.MessageDateTime.ToString() + " " + message.UserName + " says " + message.Message;
            Assert.That(messages.Count, Is.EqualTo(1));
            Assert.That(messages.First(), Is.EqualTo(expectedValue));

            _messageProducerMock.Verify(x => x.SendMessage(It.IsAny<ChatMessage>()), Times.Never);
            Assert.Pass();
        }

    }
}