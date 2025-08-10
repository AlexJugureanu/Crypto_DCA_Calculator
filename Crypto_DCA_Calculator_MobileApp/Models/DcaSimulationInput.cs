namespace Crypto_DCA_Calculator_MobileApp.Models;

public class DcaSimulationInput
{
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public double MonthlyAmount { get; set; }
	public CryptoType CryptoCoin { get; set; } = new CryptoType();
	public int InvestDayOfTheMonth { get; set; }
}