using Crypto_DCA_Calculator_MobileApp.Models;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface IDcaCalculatorService
{
	Task<List<DcaResultForUser>> CalculateDca(DcaSimulationInput input);
}

public class DcaCalculatorService(ICryptoDataService cryptoDataService) : IDcaCalculatorService
{
	public async Task<List<DcaResultForUser>> CalculateDca(DcaSimulationInput input)
	{
		var cryptoPrices = await cryptoDataService.GetMonthlyDcaPricesAsync(
			input.CryptoCoin.Id,
			new DateTimeOffset(input.StartDate).ToUnixTimeSeconds(),
			new DateTimeOffset(input.EndDate).ToUnixTimeSeconds(),
			input.InvestDayOfTheMonth);

		var dcaResultForUserList = new List<DcaResultForUser>();
		var index = 1;
		var totalCoinsOwned = 0.0;

		foreach (var cryptoPrice in cryptoPrices)
		{

			totalCoinsOwned += input.MonthlyAmount / cryptoPrice.Value;
			var dcaResultForUser = new DcaResultForUser
			{
				Date = cryptoPrice.Key.Month.ToString() + "/" + cryptoPrice.Key.Year.ToString(),
				InvestedAmount = input.MonthlyAmount * index,
				CryptoCurrencyAmount = Math.Round(totalCoinsOwned, 5),
				ValueToday = Math.Round(totalCoinsOwned * cryptoPrice.Value, 5),
				ROI = Math.Round((totalCoinsOwned * cryptoPrice.Value - input.MonthlyAmount * index) / (input.MonthlyAmount * index) * 100, 2) + "%"
			};

			index++;

			dcaResultForUserList.Add(dcaResultForUser);
		}

		return dcaResultForUserList;
	}
}