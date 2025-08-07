using System.ComponentModel;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class DcaSimulatorPage : ContentPage, INotifyPropertyChanged
{
	private List<string> _cryptoCurrencies = [ "Bitcoin (BTC)", "Ethereum (ETH)", "Solana (SOL)" ];

	public List<string> CryptoCurrencies
	{
		get => _cryptoCurrencies;
		set
		{
			if (_cryptoCurrencies != value)
			{
				_cryptoCurrencies = value;
				OnPropertyChanged(nameof(CryptoCurrencies));
			}
		}
	}

	private string _selectedCryptoCurrency;

	public string SelectedCryptoCurrency
	{
		get => _selectedCryptoCurrency;
		set
		{
			if (_selectedCryptoCurrency != value)
			{
				_selectedCryptoCurrency = value;
				OnPropertyChanged(nameof(SelectedCryptoCurrency));
			}
		}
	}

	public DcaSimulatorPage()
	{
		InitializeComponent();

		BindingContext = this;

		SelectedCryptoCurrency = _cryptoCurrencies[0];
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}