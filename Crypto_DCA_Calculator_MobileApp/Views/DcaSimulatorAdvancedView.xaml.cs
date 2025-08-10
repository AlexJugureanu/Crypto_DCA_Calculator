using Crypto_DCA_Calculator_MobileApp.ViewModels;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class DcaSimulatorAdvancedView : ContentPage
{
	public DcaSimulatorAdvancedView(DcaSimulatorAdvancedViewModel vm)
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