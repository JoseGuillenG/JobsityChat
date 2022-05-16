using Jobsity.Chat.Bot.MessageBroker.Producer.Abstractions;
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

        public async Task ProcessStockMessageAsync(string stockCode)
        {
            try
            {
                var csvFileContent = await _client.GetAsync(stockCode);
                var stockInformation = ProcessCsvFile(csvFileContent);
                var messageToSend = $"{stockCode} quote is ${stockInformation.Close} per share.";
                _messageProducer.SendMessage(messageToSend);
            }
            catch (Exception ex)
            {

            }
        }

        private StockInformation ProcessCsvFile(string csvFileContent)
        {
            // I'm assuming the result will always be title on the first row and the information of the stock on the second one
            var title = true;
            StockInformation result = null;
            using (var reader = new StringReader(csvFileContent))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (title)
                    {
                        title = false;
                        continue;
                    }

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

                    break;
                }
            }

            return result;
        }
    }
}
