using Crypto_DCA_Calculator_MobileApp.Services;
using Microsoft.Extensions.Logging;

namespace Crypto_DCA_Calculator_MobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddTransient<IDcaCalculatorService, DcaCalculatorService>();
			builder.Services.AddTransient<ICryptoDataService, CryptoDataService>();

			builder
				.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
