using Crypto_DCA_Calculator_MobileApp.Views;

namespace Crypto_DCA_Calculator_MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

			Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
			Routing.RegisterRoute(nameof(DcaSimulatorPage), typeof(DcaSimulatorPage));

		}
	}
}
