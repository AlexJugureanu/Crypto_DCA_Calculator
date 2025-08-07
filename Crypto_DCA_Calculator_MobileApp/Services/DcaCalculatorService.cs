using Crypto_DCA_Calculator_MobileApp.Models;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface IDcaCalculatorService
{
	DcaResult CalculateDca(DcaSimulationInput input);
}

public class DcaCalculatorService : IDcaCalculatorService
{
	private readonly List<int> _dcaValues = [100, 104, 108, 112, 109, 120, 129, 140, 131, 136, 137, 137, 148, 150, 159, 190, 220, 207, 201, 206, 208];

	public DcaResult CalculateDca(DcaSimulationInput input)
	{
		var numberOfMonths = GetNumberOfMonths(input.StartDate, input.EndDate);
		if (numberOfMonths <= 0)
			return new DcaResult();

		var totalNumberOfCryptosBought = 0m;

		for (var index = 0; index <= numberOfMonths; index++)
		{
			totalNumberOfCryptosBought += input.MonthlyAmount / _dcaValues[index];
		}

		return new DcaResult
		{
			InvestDayOfTheMonth = input.InvestDayOfTheMonth,
			AmountInvested = input.MonthlyAmount * numberOfMonths,
			ValueToday = totalNumberOfCryptosBought * _dcaValues[numberOfMonths],
			ProfitLoss = (totalNumberOfCryptosBought * _dcaValues[numberOfMonths]) - (input.MonthlyAmount * numberOfMonths)
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