using Crypto_DCA_Calculator_MobileApp.Services;
using Crypto_DCA_Calculator_MobileApp.ViewModels;
using Microsoft.Extensions.Logging;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;

namespace Crypto_DCA_Calculator_MobileApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();

			SyncfusionLicenseProvider.RegisterLicense("Mzk4NDcxNUAzMzMwMmUzMDJlMzAzYjMzMzAzYkZ3V1hpZWdpZGxmRjc4MVd5NmtsVXJTUjZzVllBVlJjWDFWSXp4NEtYSVk9\r\n");

			builder.Services.AddTransient<DcaSimulatorViewModel>();
			builder.Services.AddTransient<LogInViewModel>();

			builder.Services.AddSingleton<IDcaCalculatorService, DcaCalculatorService>();
			builder.Services.AddSingleton<ICryptoDataService, CryptoDataService>();
			builder.Services.AddSingleton<IAuthService>(s =>
				new SupabaseAuthService(
					"https://fxtoygsktwmdispcckeq.supabase.co",
					"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ4dG95Z3NrdHdtZGlzcGNja2VxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTQ3MjQ3MzMsImV4cCI6MjA3MDMwMDczM30.D43438P-0vAaOzmUKuqUNXEKinqrMRdNr-caYsS36Co"
				)
			);

			builder
				.UseMauiApp<App>()
				.ConfigureSyncfusionCore()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					fonts.AddFont("NataSans-VariableFont_wght.ttf", "Font1");
				});

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
