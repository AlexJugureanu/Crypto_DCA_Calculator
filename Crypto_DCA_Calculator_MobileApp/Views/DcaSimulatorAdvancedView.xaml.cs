using Crypto_DCA_Calculator_MobileApp.ViewModels;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class DcaSimulatorAdvancedView : ContentPage
{
	public DcaSimulatorAdvancedView(DcaSimulatorAdvancedViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await ((DcaSimulatorAdvancedViewModel)BindingContext).InitializeCryptoCurrenciesCommand.ExecuteAsync(null);
	}

	public void OpenStartDatePicker(object sender, TappedEventArgs e)
	{
		startDatePicker.IsOpen = true;
	}

	public void OpenEndDatePicker(object sender, TappedEventArgs e)
	{
		endDatePicker.IsOpen = true;
	}
}