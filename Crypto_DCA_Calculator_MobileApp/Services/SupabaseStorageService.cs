using Crypto_DCA_Calculator_MobileApp.Models.DTO;
using Supabase;

namespace Crypto_DCA_Calculator_MobileApp.Services;

public interface IStorageService
{
	Task<List<CryptoCoin>> GetCryptoCoinsAsync();
}

public class SupabaseStorageService : IStorageService
{
	private Client _client;

	public SupabaseStorageService(string url, string anonKey)
	{
		_client = new Client(url, anonKey, new SupabaseOptions { AutoConnectRealtime = false });
	}

	public async Task<List<CryptoCoin>> GetCryptoCoinsAsync()
	{
		var response = await _client
			.From<CryptoCoin>()
			.Get();
		return response.Models;
	}
}