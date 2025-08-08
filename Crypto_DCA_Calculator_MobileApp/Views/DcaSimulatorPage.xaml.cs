using Crypto_DCA_Calculator_MobileApp.Models;
using Crypto_DCA_Calculator_MobileApp.Services;
using System.ComponentModel;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class DcaSimulatorPage : ContentPage, INotifyPropertyChanged
{
	private readonly IDcaCalculatorService _dcaCalculatorService;

	public List<int> DaysToChooseFrom { get; set; } = [15, 20, 25];
	private List<CryptoType> _cryptoCurrencies = [new CryptoType { Id = "bitcoin", Name = "Bitcoin (BTC)" }];
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

	private DateTime _selectedStartDate;
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

	private DateTime _selectedEndDate;
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

	public string SelectedStartMonthYear => SelectedStartDate.ToString("MM/yyyy");
	public string SelectedEndMonthYear => SelectedEndDate.ToString("MM/yyyy");


	public DcaSimulationInput DcaSimulationInput { get; set; } = new();
	public DcaResult DcaSimulationResult { get; set; } = new();

	public DcaSimulatorPage(IDcaCalculatorService dcaCalculatorService)
	{
		BindingContext = this;

		_dcaCalculatorService = dcaCalculatorService;

		SelectedStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
		SelectedEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);

		InitializeComponent();
	}

	public void OpenStartDatePicker(object sender, TappedEventArgs e)
	{
		startDatePicker.IsOpen = true;
	}

	public void OpenEndDatePicker(object sender, TappedEventArgs e)
	{
		endDatePicker.IsOpen = true;
	}

	private async void CalculateDca(object sender, EventArgs e)
	{
		DcaSimulationResult = await _dcaCalculatorService.CalculateDca(DcaSimulationInput);
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}