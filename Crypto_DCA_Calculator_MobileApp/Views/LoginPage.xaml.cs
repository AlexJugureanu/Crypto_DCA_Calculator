using Crypto_DCA_Calculator_MobileApp.Services;
using System.ComponentModel;

namespace Crypto_DCA_Calculator_MobileApp.Views;

public partial class LoginPage : ContentPage, INotifyPropertyChanged
{
	private readonly IAuthService _authService;

	private string _email = string.Empty;
	public string Email
	{
		get => _email;
		set
		{
			if (_email != value)
			{
				_email = value;
				OnPropertyChanged(nameof(Email));
			}
		}
	}

	private string _password = string.Empty;
	public string Password
	{
		get => _password;
		set
		{
			if (_password != value)
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}
	}

	public LoginPage(IAuthService authService)
	{
		BindingContext = this;

		_authService = authService;

		InitializeComponent();
	}

	private async void Login(object sender, EventArgs e)
	{
		var result = await _authService.LogInAsync(Email, Password);
	}
}