using Crypto_DCA_Calculator_MobileApp.Models;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface IDcaCalculatorService
{
	Task<(DcaResult, List<DcaResultForUser>)> CalculateDca(DcaSimulationInput input);
}

public class DcaCalculatorService(ICryptoDataService cryptoDataService) : IDcaCalculatorService
{
	public async Task<(DcaResult, List<DcaResultForUser>)> CalculateDca(DcaSimulationInput input)
	{
		var cryptoPrices = await cryptoDataService.GetMonthlyDcaPricesAsync(
			input.CryptoCoin.Id,
			new DateTimeOffset(input.StartDate).ToUnixTimeSeconds(),
			new DateTimeOffset(input.EndDate).ToUnixTimeSeconds(),
			input.InvestDayOfTheMonth);

		var totalNumberOfCryptosBought2 = 0.0;

		foreach (var price in cryptoPrices)
		{
			totalNumberOfCryptosBought2 += input.MonthlyAmount / price.Value;
		}

		var numberOfMonths = GetNumberOfMonths(input.StartDate, input.EndDate);
		//if (numberOfMonths <= 0)
		//	return new (DcaResult, DcaResultForUser)();

		var dcaResult = new DcaResult
		{
			InvestDayOfTheMonth = input.InvestDayOfTheMonth,
			AmountInvested = input.MonthlyAmount * numberOfMonths,
			ValueToday = totalNumberOfCryptosBought2 * cryptoPrices.FirstOrDefault(x => x.Key.Month == DateTime.Now.Month && x.Key.Year == DateTime.Now.Year).Value,
			ProfitLoss = (totalNumberOfCryptosBought2 * cryptoPrices.FirstOrDefault(x => x.Key.Month == DateTime.Now.Month && x.Key.Year == DateTime.Now.Year).Value) - (input.MonthlyAmount * numberOfMonths)
		};

		var currentValue = (totalNumberOfCryptosBought2 * cryptoPrices.FirstOrDefault(x => x.Key.Month == DateTime.Now.Month && x.Key.Year == DateTime.Now.Year).Value);
		var amountInvested = input.MonthlyAmount * numberOfMonths;
		var roi = (currentValue - amountInvested) / amountInvested * 100;

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

		return (dcaResult, dcaResultForUserList);
	}

	private static int GetNumberOfMonths(DateTime startDate, DateTime endDate)
	{
		int months = 0;
		while (startDate < endDate)
		{
			startDate = startDate.AddMonths(1);
			months++;
		}
		return months;
	}
}