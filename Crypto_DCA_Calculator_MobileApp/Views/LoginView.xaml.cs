using Crypto_DCA_Calculator_MobileApp.ViewModels;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LogInViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}