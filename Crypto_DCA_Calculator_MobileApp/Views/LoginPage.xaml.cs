using Crypto_DCA_Calculator_MobileApp.ViewModels;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LogInViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}