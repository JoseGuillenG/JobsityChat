using Jobsity.Chat.Api.Application.Chat;
using Jobsity.Chat.Api.Models;
using Jobsity.Chat.Api.SignalR;
using Moq;
using SignalR_UnitTestingSupportCommon.IHubContextSupport;

namespace Jobsity.Chat.Api.Aplication.Tests.Chat
{
    public class BotChatProcessorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ProcessMessageAsync_CorrectMessage_Success()
        {
            // Arrange
            var iHubContextSupport = new UnitTestingSupportForIHubContext<ChatHub>();

            var message = new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Jose",
                Message = "Message",
                MessageDateTime = DateTime.Now,
                Code = String.Empty,
                ChatRoom = String.Empty
            };

            var botChatProcessor = new BotChatProcessor(iHubContextSupport.IHubContextMock.Object);

            // Act
            await botChatProcessor.ProcessMessageAsync(message);

            // Assert
            var expectedSender = message.MessageDateTime.ToString() + " " + "Bot";
            iHubContextSupport.ClientsAllMock
                .Verify(x => x.SendCoreAsync(
                    "ReceiveMessage",
                    new object[] { expectedSender, message.Message },
                    It.IsAny<CancellationToken>())
                );

            Assert.Pass();
        }

        [Test]
        public void ProcessMessageAsync_NullMessage_Fail()
        {
            // Arrange
            var iHubContextSupport = new UnitTestingSupportForIHubContext<ChatHub>();
            var botChatProcessor = new BotChatProcessor(iHubContextSupport.IHubContextMock.Object);

            // Act
            // Assert
            Assert.ThrowsAsync<Exception>(() => botChatProcessor.ProcessMessageAsync(null));
        }
    }
}