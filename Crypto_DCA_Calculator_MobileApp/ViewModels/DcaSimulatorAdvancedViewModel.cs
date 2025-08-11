using Crypto_DCA_Calculator_MobileApp.Models;
using Crypto_DCA_Calculator_MobileApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Crypto_DCA_Calculator_MobileApp.ViewModels;

public class DcaSimulatorAdvancedViewModel : INotifyPropertyChanged
{
	private readonly IDcaCalculatorService _dcaCalculatorService;
	private readonly ISupabaseStorageService _supabaseStorageService;


	public event PropertyChangedEventHandler? PropertyChanged;

	private string _statusMessage = string.Empty;
	private bool _isBusy;
	private DateTime _selectedStartDate;
	private DateTime _selectedEndDate;
	private List<DcaResultForUserUi> _dcaSimListResult = [];

	private ObservableCollection<CryptoType> _cryptoCurrencies = [];
	private ObservableCollection<CryptoType> _selectedCryptoCurrencies = [];
	private CryptoType _selectedCryptoCurrency = new();

	private List<DcaSimulationInput> dcaSimulationInputList = [];
	private DcaSimulationInput _dcaSimulationInput = new();

	private Dictionary<CryptoType, List<DcaResultForUserUi>> _listResult = [];

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

	public ObservableCollection<CryptoType> CryptoCurrencies
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

	public ObservableCollection<CryptoType> SelectedCryptoCurrencies
	{
		get => _selectedCryptoCurrencies;
		set
		{
			if (_selectedCryptoCurrencies != value)
			{
				_selectedCryptoCurrencies = value;
				OnPropertyChanged(nameof(SelectedCryptoCurrencies));
			}
		}
	}

	public CryptoType SelectedCryptoCurrency
	{
		get => _selectedCryptoCurrency;
		set
		{
			if (_selectedCryptoCurrency != value)
			{
				_selectedCryptoCurrency = value;
				OnPropertyChanged(nameof(SelectedCryptoCurrency));
				DcaSimListResult = ListResult.FirstOrDefault(x => x.Key == SelectedCryptoCurrency).Value;
			}
		}
	}

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

	public DcaSimulationInput DcaSimulationInput
	{
		get => _dcaSimulationInput;
		set
		{
			if (_dcaSimulationInput != value)
			{
				_dcaSimulationInput = value;
				OnPropertyChanged(nameof(DcaSimulationInput));
			}
		}
	}

	public Dictionary<CryptoType, List<DcaResultForUserUi>> ListResult
	{
		get => _listResult;
		set
		{
			if (_listResult != value)
			{
				_listResult = value;
				OnPropertyChanged(nameof(ListResult));
			}
		}
	}

	public ICommand CalculateDataCommand { get; }
	public ICommand AddCryptoCommand { get; }
	public IAsyncRelayCommand InitializeCryptoCurrenciesCommand { get; }

	public DcaSimulatorAdvancedViewModel(
		IDcaCalculatorService dcaCalculatorService,
		ISupabaseStorageService supabaseStorageService)
	{
		_dcaCalculatorService = dcaCalculatorService;
		_supabaseStorageService = supabaseStorageService;

		CalculateDataCommand = new Command(async () => await CalculateDataAsync(), () => !IsBusy);
		AddCryptoCommand = new Command(AddCrypto);
		InitializeCryptoCurrenciesCommand = new AsyncRelayCommand(InitializeCryptoCurrenciesList, () => !IsBusy);

		SelectedStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
		SelectedEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
	}

	private async Task CalculateDataAsync()
	{
		if (IsBusy)
			return;

		IsBusy = true;

		try
		{
			List<DcaResultForUser> response;
			foreach (var simInput in dcaSimulationInputList)
			{
				response = await _dcaCalculatorService.CalculateDca(simInput);
				var newUiResult = new List<DcaResultForUserUi>();

				foreach (var record in response)
				{
					newUiResult.Add(new DcaResultForUserUi
					{
						CryptoCurrencyAmount = record.CryptoCurrencyAmount.ToString(),
						CryptoName = record.CryptoName,
						Date = record.Date,
						InvestedAmount = record.InvestedAmount,
						ROI = record.ROI + "%",
						ValueToday = record.ValueToday,
					});
				}

				ListResult.Add(simInput.CryptoCoin, newUiResult);
			}
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

	public void AddCrypto()
	{
		dcaSimulationInputList.Add(DcaSimulationInput);
		SelectedCryptoCurrencies.Add(DcaSimulationInput.CryptoCoin);
		CryptoCurrencies.Remove(DcaSimulationInput.CryptoCoin);

		DcaSimulationInput = new DcaSimulationInput
		{
			//it wont work if the current month is the first of the year
			StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1),
			EndDate = DateTime.Now,
			MonthlyAmount = 0,
			CryptoCoin = new(),
			InvestDayOfTheMonth = DaysToChooseFrom[0]
		};
	}

	private async Task InitializeCryptoCurrenciesList()
	{
		var cryptCoins = await _supabaseStorageService.GetCryptoCoinsAsync();

		CryptoCurrencies = new ObservableCollection<CryptoType>(cryptCoins.Select(x => new CryptoType
		{
			Name = x.Name,
			Id = x.CryptoId
		}));
	}

	protected void OnPropertyChanged([CallerMemberName] string name = null)
	=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}