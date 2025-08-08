namespace Crypto_DCA_Calculator_MobileApp.Models;

public class DcaSimulationInput
{
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public decimal MonthlyAmount { get; set; }
	public string CryptoId { get; set; } = string.Empty;
	public int InvestDayOfTheMonth { get; set; }
}