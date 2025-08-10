using Crypto_DCA_Calculator_MobileApp.ViewModels;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class DcaSimulatorIntermediateView : ContentPage
{
	public DcaSimulatorIntermediateView(DcaSimulatorIntermediateViewModel vm)
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

	private void Button_Clicked(object sender, EventArgs e)
	{
		if (CryptoListView.IsVisible)
		{
			CryptoListView.IsVisible = false;
			ManipulateListView_Btn.Text = "Select crypto";
		}
		else
		{
			CryptoListView.IsVisible = true;
			ManipulateListView_Btn.Text = "Confirm selection";
		}
	}
}