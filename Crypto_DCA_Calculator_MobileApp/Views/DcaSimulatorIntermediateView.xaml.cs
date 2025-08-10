namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class DcaSimulatorIntermediateView : ContentPage
{
	public DcaSimulatorIntermediateView()
	{
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
}
