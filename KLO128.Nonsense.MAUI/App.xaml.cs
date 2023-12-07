using FreshMvvm.Maui;
using KLO128.Nonsense.MAUI.PageModels;

namespace KLO128.Nonsense.MAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
	}
}
