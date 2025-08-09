using Crypto_DCA_Calculator_MobileApp.Views;

namespace Crypto_DCA_Calculator_MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

			Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
			Routing.RegisterRoute(nameof(DcaSimulatorView), typeof(DcaSimulatorView));

		}
	}
}
