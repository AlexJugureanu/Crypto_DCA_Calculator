using Crypto_DCA_Calculator_MobileApp.Models;
using Crypto_DCA_Calculator_MobileApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Crypto_DCA_Calculator_MobileApp.ViewModels;

public class DcaSimulatorIntermediateViewModel : INotifyPropertyChanged
{
	private readonly IDcaCalculatorService _dcaCalculatorService;

	public event PropertyChangedEventHandler? PropertyChanged;

	private string _statusMessage = string.Empty;
	private bool _isBusy;
	private DateTime _selectedStartDate;
	private DateTime _selectedEndDate;
	private List<DcaResultForUserUi> _dcaSimListResult = [];
	private string _percentageForEachCrypto = string.Empty;
	public List<int> DaysToChooseFrom { get; set; } = [15, 20, 25];
	public string SelectedStartMonthYear => SelectedStartDate.ToString("MM/yyyy");
	public string SelectedEndMonthYear => SelectedEndDate.ToString("MM/yyyy");

	public string StatusMessage
	{
		get => _statusMessage;
		set { _statusMessage = value; OnPropertyChanged(); }
	}

	public bool IsBusy
	{
		get => _isBusy;
		set { _isBusy = value; OnPropertyChanged(); }
	}

	public DateTime SelectedStartDate
	{
		get => _selectedStartDate;
		set
		{
			if (_selectedStartDate != value)
			{
				_selectedStartDate = new DateTime(value.Year, value.Month, 1); ;
				OnPropertyChanged(nameof(SelectedStartDate));
				OnPropertyChanged(nameof(SelectedStartMonthYear));
				DcaSimulationInput.StartDate = value;
			}
		}
	}

	public DateTime SelectedEndDate
	{
		get => _selectedEndDate;
		set
		{
			if (_selectedEndDate != value)
			{
				_selectedEndDate = new DateTime(value.Year, value.Month, 1); ;
				OnPropertyChanged(nameof(SelectedEndDate));
				OnPropertyChanged(nameof(SelectedEndMonthYear));
				DcaSimulationInput.EndDate = value;
			}
		}
	}

	public List<CryptoTypeUi> AvailableCryptoCurrencies { get; set; } =
		[new CryptoTypeUi { Id = "bitcoin", Name = "Bitcoin (BTC)" },
		new CryptoTypeUi { Id = "ethereum", Name = "Ethereum (ETH)" },
		new CryptoTypeUi { Id = "solana", Name = "Solana (SLN)" },
		new CryptoTypeUi { Id = "ripple", Name = "Ripple (RPL)" }];

	public DcaSimulationInputIntermediate DcaSimulationInput { get; set; } = new();

	public List<DcaResultForUserUi> DcaSimListResult
	{
		get => _dcaSimListResult;
		set
		{
			if (_dcaSimListResult != value)
			{
				_dcaSimListResult = value;
				OnPropertyChanged(nameof(DcaSimListResult));
			}
		}
	}

	public string PercentageForEachCrypto
	{
		get => _percentageForEachCrypto;
		set
		{
			if (_percentageForEachCrypto != value)
			{
				_percentageForEachCrypto = value;
				OnPropertyChanged(nameof(PercentageForEachCrypto));
			}
		}
	}

	public ICommand CalculateDataCommand { get; }
	public ICommand ConfirmCryptoSelectionCommand { get; }

	public DcaSimulatorIntermediateViewModel(IDcaCalculatorService dcaCalculatorService)
	{
		_dcaCalculatorService = dcaCalculatorService;

		CalculateDataCommand = new Command(async () => await CalculateDataAsync(), () => !IsBusy);
		ConfirmCryptoSelectionCommand = new Command(ConfirmCryptoSelection);

		SelectedStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
		SelectedEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);

		DcaSimulationInput.InvestDayOfTheMonth = DaysToChooseFrom[0];
	}

	private async Task CalculateDataAsync()
	{
		if (IsBusy)
			return;

		IsBusy = true;

		try
		{
			var result = await _dcaCalculatorService.CalculateDcaIntermediate(DcaSimulationInput);
			if (result == null || result.Count == 0)
			{
				// pop up for fail
				StatusMessage = "Calculation failed. Reason? idk";
			}

			UpdateDcaSimListResult(result);
		}
		catch (Exception ex)
		{
			StatusMessage = "Calculation failed with the following message: " + ex.Message;
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ConfirmCryptoSelection()
	{
		DcaSimulationInput.CryptoCoins = [.. AvailableCryptoCurrencies.Where(crypto => crypto.IsSelected).Select(x => new CryptoType { Id = x.Id, Name = x.Name })];
		if (DcaSimulationInput.CryptoCoins.Count == 0)
		{
			PercentageForEachCrypto = string.Empty;
			return;
		}

		PercentageForEachCrypto = 100 / DcaSimulationInput.CryptoCoins.Count + "%";
	}

	private void UpdateDcaSimListResult(List<DcaResultForUser> result)
	{
		var dcaGroupedByDate = result.GroupBy(x => x.Date).ToList();

		var newList = new List<DcaResultForUserUi>();
		foreach (var dca in dcaGroupedByDate)
		{


			var investedAmount = 0.0;
			var cryptoAmount = string.Empty;
			var valueToday = 0.0;
			var roi = 0.0;

			foreach (var details in dca)
			{
				investedAmount += details.InvestedAmount;
				cryptoAmount += details.CryptoCurrencyAmount + " " + details.CryptoName + "\n";
				valueToday += details.ValueToday;
				roi += details.ROI;
			}

			var newResult = new DcaResultForUserUi
			{
				Date = dca.Key,
				ValueToday = valueToday,
				CryptoCurrencyAmount = cryptoAmount,
				InvestedAmount = investedAmount,
				ROI = roi + "%",
			};

			newList.Add(newResult);
		}

		DcaSimListResult = newList;
	}

	protected void OnPropertyChanged([CallerMemberName] string name = null)
	=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}