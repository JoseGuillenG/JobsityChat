using Jobsity.Chat.Bot.MessageBroker.Producer.Abstractions;
using Jobsity.Chat.Bot.Models.Messages;
using Jobsity.Chat.Bot.Models.Stock;
using Jobsity.Chat.Bot.RestClient.Abstractions.Client;

namespace Jobsity.Chat.Bot.Application.Stock
{
    public class StockProcessor: IStockProcessor
    {
        private readonly IClientFactory _client;
        private readonly IMessageProducer _messageProducer;

        public StockProcessor(IClientFactory client, IMessageProducer messageProducer)
        {
            _client = client;
            _messageProducer = messageProducer;
        }

        public async Task ProcessStockMessageAsync(ChatMessage message)
        {
            try
            {
                var csvFileContent = await _client.GetAsync(message.Code);
                var stockInformation = ProcessCsvFile(csvFileContent);
                var messageToSend = string.Empty;
                if (stockInformation.Close.Equals("N/D"))
                    messageToSend = $"{message.Code} quote hasn't been found.";
                else
                    messageToSend = $"{message.Code} quote is ${stockInformation.Close} per share.";
                message.Message = messageToSend;
                _messageProducer.SendMessage(message);
            }
            catch (Exception ex)
            {
                var messageToSend = $"{message.UserName} I couldnt process your command \"{message.Code}\" on the message \"{message.Message}\"";
                message.Message = messageToSend;
                _messageProducer.SendMessage(message);
            }
        }

        private StockInformation ProcessCsvFile(string csvFileContent)
        {
            // I'm assuming the result will always be title on the first row and the information of the stock on the second one
            StockInformation result = null;
            using (var reader = new StringReader(csvFileContent))
            {
                var line = reader.ReadLine();
                line = reader.ReadLine();
                var values = line.Split(',');
                result = new StockInformation
                {
                    Code = values[0],
                    Date = values[1],
                    Time = values[2],
                    Open = values[3],
                    High = values[4],
                    Low = values[5],
                    Close = values[6],
                    Volume = values[7]
                };
            }

            return result;
        }
    }
}
