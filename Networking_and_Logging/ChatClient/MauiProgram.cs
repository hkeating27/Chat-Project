using Microsoft.Extensions.Logging;

namespace ChatClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.Services.AddLogging(configure =>
			 {
				 configure.AddDebug();
				 configure.AddProvider(new FileLogger());
				 configure.SetMinimumLevel(LogLevel.Debug);
			 })
			.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
