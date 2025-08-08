using Crypto_DCA_Calculator_MobileApp.Models;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface IDcaCalculatorService
{
	Task<DcaResult> CalculateDca(DcaSimulationInput input);
}

public class DcaCalculatorService(ICryptoDataService cryptoDataService) : IDcaCalculatorService
{
	public async Task<DcaResult> CalculateDca(DcaSimulationInput input)
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
		if (numberOfMonths <= 0)
			return new DcaResult();

		return new DcaResult
		{
			InvestDayOfTheMonth = input.InvestDayOfTheMonth,
			AmountInvested = input.MonthlyAmount * numberOfMonths,
			ValueToday = totalNumberOfCryptosBought2 * cryptoPrices.FirstOrDefault(x => x.Key.Month == DateTime.Now.Month && x.Key.Year == DateTime.Now.Year).Value,
			ProfitLoss = (totalNumberOfCryptosBought2 * cryptoPrices.FirstOrDefault(x => x.Key.Month == DateTime.Now.Month && x.Key.Year == DateTime.Now.Year).Value) - (input.MonthlyAmount * numberOfMonths)
		};
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