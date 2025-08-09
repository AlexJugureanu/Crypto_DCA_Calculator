using Crypto_DCA_Calculator_MobileApp.ViewModels;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class DcaSimulatorPage : ContentPage
{
	public DcaSimulatorPage(DcaSimulatorViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
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