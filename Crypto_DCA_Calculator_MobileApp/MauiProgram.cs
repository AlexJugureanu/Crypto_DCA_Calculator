using Crypto_DCA_Calculator_MobileApp.Services;
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

			builder.Services.AddTransient<IDcaCalculatorService, DcaCalculatorService>();
			builder.Services.AddTransient<ICryptoDataService, CryptoDataService>();

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
