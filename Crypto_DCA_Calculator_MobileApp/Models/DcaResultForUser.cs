namespace Crypto_DCA_Calculator_MobileApp.Models;

public class DcaResultForUser
{
	public string Date { get; set; } = string.Empty;
	public double InvestedAmount { get; set; }
	public double CryptoCurrencyAmount { get; set; }
	public double ValueToday { get; set; }
	public string ROI { get; set; } = string.Empty;
}