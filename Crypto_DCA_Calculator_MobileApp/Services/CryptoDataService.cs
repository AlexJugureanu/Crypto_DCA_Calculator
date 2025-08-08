using System.Text.Json;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface ICryptoDataService
{
	 Task<Dictionary<DateTime, double>> GetMonthlyDcaPricesAsync(
		string coinId,
		long startTimestamp,
		long endTimestamp,
		int dcaDay,
		string currency = "eur");
}

public class CryptoDataService : ICryptoDataService
{
	private static readonly HttpClient httpClient = new HttpClient();

	public async Task<Dictionary<DateTime, double>> GetMonthlyDcaPricesAsync(
	string coinId,
	long startTimestamp,
	long endTimestamp,
	int dcaDay,
	string currency = "eur")
	{
		var url = $"https://api.coingecko.com/api/v3/coins/{coinId}/market_chart/range?vs_currency={currency}&from={startTimestamp}&to={endTimestamp}";
		var response = await httpClient.GetAsync(url);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		using var doc = JsonDocument.Parse(json);

		var rawPrices = new List<(DateTime Date, double Price)>();

		if (doc.RootElement.TryGetProperty("prices", out var priceArray))
		{
			foreach (var item in priceArray.EnumerateArray())
			{
				var timestamp = item[0].GetDouble();
				var price = item[1].GetDouble();
				var date = DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp).DateTime.Date;
				rawPrices.Add((date, price));
			}
		}

		var groupedPrices = rawPrices
			.GroupBy(p => new { p.Date.Year, p.Date.Month })
			.Select(g =>
			{
				var year = g.Key.Year;
				var month = g.Key.Month;
				var day = Math.Min(dcaDay, DateTime.DaysInMonth(year, month));
				var targetDate = new DateTime(year, month, day);

				var closest = g.OrderBy(p => Math.Abs((p.Date - targetDate).TotalDays)).First();
				return new KeyValuePair<DateTime, double>(closest.Date, closest.Price);
			});

		return groupedPrices.ToDictionary(kv => kv.Key, kv => kv.Value);
	}
}