using Crypto_DCA_Calculator_MobileApp.Services;
using Crypto_DCA_Calculator_MobileApp.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Crypto_DCA_Calculator_MobileApp.ViewModels;

public class LogInViewModel : INotifyPropertyChanged
{
	private readonly IAuthService _authService;

	public event PropertyChangedEventHandler? PropertyChanged;


	private string _statusMessage = string.Empty;
	private bool _isBusy;
	private string _email = string.Empty;
	private string _password = string.Empty;

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

	public ICommand LogInCommand { get; }

	public LogInViewModel(IAuthService authService)
	{
		_authService = authService;

		LogInCommand = new Command(async () => await LogInAsync(), () => !IsBusy);
	}

	private async Task LogInAsync()
	{
		if (IsBusy)
			return;

		IsBusy = true;

		try
		{
			if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
				StatusMessage = "Email and password can not be empty, please verify.";

			var result = await _authService.LogInAsync(Email, Password);
			if (!result)
				StatusMessage = "Log in failed, please check your email and password.";

			await Shell.Current.GoToAsync(nameof(DcaSimulatorPage));
		}
		catch (Exception ex)
		{
			StatusMessage = "Log in failed with the following message: " + ex.Message;
		}
		finally
		{
			IsBusy = false;
		}
	}

	protected void OnPropertyChanged([CallerMemberName] string name = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}