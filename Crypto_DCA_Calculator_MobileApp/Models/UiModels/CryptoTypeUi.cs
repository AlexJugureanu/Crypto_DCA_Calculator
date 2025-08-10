using System.ComponentModel;

namespace Crypto_DCA_Calculator_MobileApp.Models;

public class CryptoTypeUi : INotifyPropertyChanged
{
	public string Id { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;

	private bool _isSelected;
	public bool IsSelected
	{
		get => _isSelected;
		set
		{
			if (_isSelected != value)
			{
				_isSelected = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;
}