using System.Text.Json;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface ICryptoDataService
{
	Task<Dictionary<DateTime, double>> GetMonthlyDcaPricesAsync(string coinId, string currency = "eur");
}

public class CryptoDataService : ICryptoDataService
{
	private static readonly HttpClient httpClient = new HttpClient();

	public async Task<Dictionary<DateTime, double>> GetMonthlyDcaPricesAsync(string coinId, string currency = "eur")
	{
		var url = $"https://api.coingecko.com/api/v3/coins/{coinId}/market_chart?vs_currency={currency}&days=365&interval=daily";
		var response = await httpClient.GetAsync(url);

		//will throw an exception if the response is not successful
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		using var doc = JsonDocument.Parse(json);

		var prices = new Dictionary<DateTime, double>();

		if (doc.RootElement.TryGetProperty("prices", out var priceArray))
		{
			foreach (var item in priceArray.EnumerateArray())
			{
				var timestamp = item[0].GetDouble();
				var price = item[1].GetDouble();

				var date = DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp).DateTime.Date;

				if (!prices.ContainsKey(date))
					prices[date] = price;
			}
		}

		return prices;
	}
}