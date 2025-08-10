namespace Crypto_DCA_Calculator_MobileApp.Models;

public class DcaResultForUserUi
{
	public string Date { get; set; } = string.Empty;
	public double InvestedAmount { get; set; }
	public string CryptoCurrencyAmount { get; set; } = string.Empty;
	public double ValueToday { get; set; }
	public string ROI { get; set; } = string.Empty;
	public string CryptoName { get; set; } = string.Empty;
}