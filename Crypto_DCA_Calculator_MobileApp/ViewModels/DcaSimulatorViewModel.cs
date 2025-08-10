using Crypto_DCA_Calculator_MobileApp.Models;
using Crypto_DCA_Calculator_MobileApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Crypto_DCA_Calculator_MobileApp.ViewModels;

public class DcaSimulatorViewModel : INotifyPropertyChanged
{
	private readonly IDcaCalculatorService _dcaCalculatorService;

	public event PropertyChangedEventHandler? PropertyChanged;

	private string _statusMessage = string.Empty;
	private bool _isBusy;
	private DateTime _selectedStartDate;
	private DateTime _selectedEndDate;
	private List<DcaResultForUserUi> _dcaSimListResult = [];
	
	private List<CryptoType> _cryptoCurrencies = [new CryptoType { Id = "bitcoin", Name = "Bitcoin (BTC)" }];
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

	public List<CryptoType> CryptoCurrencies
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

	public DcaSimulationInput DcaSimulationInput { get; set; } = new();

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

	public ICommand CalculateDataCommand { get; }

	public DcaSimulatorViewModel(IDcaCalculatorService dcaCalculatorService)
	{
		_dcaCalculatorService = dcaCalculatorService;

		CalculateDataCommand = new Command(async () => await CalculateDataAsync(), () => !IsBusy);

		SelectedStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
		SelectedEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
	}

	private async Task CalculateDataAsync()
	{
		if (IsBusy)
			return;

		IsBusy = true;

		try
		{
			var result = await _dcaCalculatorService.CalculateDca(DcaSimulationInput);
			if (result == null || result.Count == 0)
			{
				// pop up for fail
				StatusMessage = "Calculation failed. Reason? idk";
			}

			foreach(var record in result)
			{
				DcaSimListResult.Add(new DcaResultForUserUi
				{
					CryptoCurrencyAmount = record.CryptoCurrencyAmount.ToString(),
					CryptoName = record.CryptoName,
					Date = record.Date,
					InvestedAmount = record.InvestedAmount,
					ROI = record.ROI + "%",
					ValueToday = record.ValueToday,
				});
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

	protected void OnPropertyChanged([CallerMemberName] string name = null)
	=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}