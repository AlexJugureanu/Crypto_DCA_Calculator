using Supabase;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface IAuthService
{
	Task<bool> LogInAsync(string email, string password);
	Task<bool> SignInsync(string email, string password);
	Task LogOutAsync();
	bool IsUserLoggedIn { get; }
}

public class SupabaseAuthService : IAuthService
{
	private readonly Client _client;

	public SupabaseAuthService(string url, string anonKey)
	{
		_client = new Client(url, anonKey, new SupabaseOptions { AutoConnectRealtime = false });
	}

	public bool IsUserLoggedIn => _client.Auth.CurrentUser != null;

	public async Task<bool> LogInAsync(string email, string password)
	{
		var session = await _client.Auth.SignIn(email, password);
		return session.User != null;
	}

	public async Task<bool> SignInsync(string email, string password)
	{
		var session = await _client.Auth.SignUp(email, password);
		return session.User != null;
	}

	public async Task LogOutAsync()
	{
		await _client.Auth.SignOut();
	}
}