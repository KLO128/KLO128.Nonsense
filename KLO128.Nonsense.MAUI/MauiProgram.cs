using CommunityToolkit.Maui;
using FreshMvvm.Maui.Extensions;
using KLO128.Nonsense.Common;
using KLO128.Nonsense.Common.Abstract;
using KLO128.Nonsense.MAUI.PageModels;
using KLO128.Nonsense.MAUI.Pages;
using KLO128.Nonsense.SQLite;

namespace KLO128.Nonsense.MAUI;

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
			}).UseMauiCommunityToolkit();


		// services
        builder.Services.AddSingleton<ILexer, CSharpLexer>();
        builder.Services.AddSingleton<ISomeAdapter, SomeSQLiteAdapter>();

        // page models
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<MainPageModel>();

        // pages
        builder.Services.AddSingleton<HomeView>();
        builder.Services.AddSingleton<MainPage>();

        var ret = builder.Build();

		ret.UseFreshMvvm();

		return ret;
	}
}
