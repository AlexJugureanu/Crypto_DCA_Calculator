using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Crypto_DCA_Calculator_MobileApp.Models.DTO;

[Table("CryptoCoins")]
public class CryptoCoin : BaseModel
{
	[PrimaryKey("id")]
	public int Id { get; set; }

	[Column("created_at")]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	[Column("name")]
	public string Name { get; set; } = string.Empty;

	[Column("crypto_id")]
	public string CryptoId { get; set; } = string.Empty;
}