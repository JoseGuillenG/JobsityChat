using Jobsity.Chat.Bot.MessageBroker.Producer.Abstractions;
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
                _messageProducer.SendMessage(stockInformation);
            }
            catch (Exception ex)
            {

            }
        }

        private string ProcessCsvFile(string csvFileContent)
        {
            using (var reader = new StreamReader(@"C:\test.csv"))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    listA.Add(values[0]);
                    listB.Add(values[1]);
                }
            }
        }
    }
}
