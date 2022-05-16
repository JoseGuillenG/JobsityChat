using Jobsity.Chat.Bot.RestClient.Abstractions.Client;

namespace Jobsity.Chat.Bot.RestClient.Client
{
    public class ClientFactory: IClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientFactory(IHttpClientFactory httpClientFactory) => 
            _httpClientFactory = httpClientFactory;

        public async Task<string> GetAsync(string stockCode)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                return contentStream;
            }

            throw new Exception("Error");
        }
    }
}
